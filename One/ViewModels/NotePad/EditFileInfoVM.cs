using Avalonia.Interactivity;

using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Helpers;
using One.Toolbox.Services;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace One.Toolbox.ViewModels.NotePad;

public partial class EditFileInfoVM : ObservableObject
{
    /// <summary> 文件名 </summary>
    [ObservableProperty]
    private string fileName;

    /// <summary> 文件名 </summary>
    [ObservableProperty]
    private bool isEditFileName;

    /// <summary> 文件后缀 </summary>
    //[ObservableProperty]
    //private string extension;

    [ObservableProperty]
    private DateTime createTime;

    [ObservableProperty]
    private DateTime modifyTime;

    [ObservableProperty]
    private TextDocument document;

    [ObservableProperty]
    private bool isDirty;

    [ObservableProperty]
    private bool isReadOnly;

    [ObservableProperty]
    private string isReadOnlyReason;

    /// <summary> 当前打开的文件路径 </summary>
    [ObservableProperty]
    private string filePath;

    /// <summary> AvalonEdit exposes a Highlighting property that controls whether keywords, comments and other interesting text parts are colored or highlighted in any other visual way. This property exposes the highlighting information for the text file managed in this viewmodel class. </summary>
    [ObservableProperty]
    private IHighlightingDefinition selectedHighlightingDefinition;

    [ObservableProperty]
    private ReadOnlyCollection<IHighlightingDefinition> highlightingDefinitionOC;

    [ObservableProperty]
    private Encoding encoding = Encoding.UTF8;

    public Action UpdateInfoAction { get; set; }

    /// <summary> UI 展示数据使用 </summary>
    public EditFileInfoVM()
    {
    }

    /// <summary> 正常使用 </summary>
    /// <param name="filePath"> </param>
    public EditFileInfoVM(string filePath)
    {
        //FileName = "未命名" + DateTime.Now.ToString("yyMMdd-HHmmss");
        //Extension = ".txt";
        //FilePath = PathHelper.dataPath + FileName + Extension;

        FilePath = filePath;
        FileName = System.IO.Path.GetFileName(FilePath);
        //Extension = System.IO.Path.GetExtension(FilePath);

        HighlightingDefinitionOC = HighlightingManager.Instance.HighlightingDefinitions;
    }

    #region RelayCommand

    [RelayCommand]
    private async void OpenFile()
    {
        //var dlg = new OpenFileDialog();
        //if (dlg.ShowDialog().GetValueOrDefault())
        //{
        //    var fileViewModel = LoadDocument(dlg.FileName);
        //}

        try
        {
            var filesService = App.Current?.Services?.GetService<IFilesService>();
            if (filesService is null) throw new NullReferenceException("Missing File Service instance.");

            var file = await filesService.OpenFileAsync();
            if (file is null) return;

            // Limit the text file to 1MB so that the demo wont lag.
            if ((await file.GetBasicPropertiesAsync()).Size <= 1024 * 1024 * 1)
            {
                await using var readStream = await file.OpenReadAsync();
                using var reader = new StreamReader(readStream);
                var FileText = await reader.ReadToEndAsync();
            }
            else
            {
                throw new Exception("File exceeded 1MB limit.");
            }
        }
        catch (Exception e)
        {
            MessageShowHelper.ShowErrorMessage(e.ToString());
        }
    }

    [RelayCommand]
    private void OpenFilePath()
    {
        Process.Start("explorer.exe", PathHelper.dataPath);
    }

    [RelayCommand]
    private void SaveFile()
    {
        SaveDocument();
    }

    [RelayCommand]
    private void OnSelectedHighlightingChanged(object obj)
    {
        var parames = obj as object[];

        if (parames == null)
            return;

        if (parames.Length != 1)
            return;

        var param = parames[0] as IHighlightingDefinition;
        if (param == null)
            return;
    }

    [RelayCommand]
    private void RenameFile(object obj)
    {
        //var parent = obj as Grid;
        //if (parent.FindName("txb1") is TextBox txb)
        //{
        //    IsEditFileName = true;
        //    txb.LostFocus += Txb_LostFocus;
        //    var res = txb.Focus();

        //}
    }

    private void Txb_LostFocus(object sender, RoutedEventArgs e)
    {
        IsEditFileName = false;
    }

    #endregion RelayCommand

    partial void OnFileNameChanged(string? oldValue, string newValue)
    {
        if (oldValue == null)
        {
            return;
        }
        if (newValue == oldValue)
        {
            return;
        }
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        FilePath = FilePath.Replace(oldValue, newValue);

        IsEditFileName = false;

        SaveDocument();

        UpdateInfoAction?.Invoke();
    }

    public void LoadDocument()
    {
        var res = LoadDocument(FilePath);

        if (!res)
        {
            MessageShowHelper.ShowErrorMessage($"{FilePath} 不存在！");
        }
    }

    /// <summary> 创建文件 </summary>
    /// <returns> </returns>
    public bool CreateNewFile()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath).Dispose();

            ModifyTime = CreateTime = DateTime.Now;
        }
        else
        {
            return false;
        }

        return true;
    }

    bool LoadDocument(string filePath)
    {
        if (File.Exists(filePath))
        {
            var hlManager = HighlightingManager.Instance;

            Document = new TextDocument();
            string extension = System.IO.Path.GetExtension(filePath);
            SelectedHighlightingDefinition = hlManager.GetDefinitionByExtension(extension);

            IsDirty = false;
            IsReadOnly = false;

            // Check file attributes and set to read-only if file attributes indicate that
            if ((System.IO.File.GetAttributes(filePath) & FileAttributes.ReadOnly) != 0)
            {
                IsReadOnly = true;
                IsReadOnlyReason = "This file cannot be edit because another process is currently writting to it.\n" +
                                   "Change the file access permissions or save the file in a different location if you want to edit it.";
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = AvaloniaEdit.Utils.FileReader.OpenStream(fs, Encoding))
                {
                    Document = new TextDocument(reader.ReadToEnd());
                }
            }

            return true;
        }

        return false;
    }

    public void SaveDocument()
    {
        if (FilePath == null)
            throw new ArgumentNullException("fileName");

        if (IsDirty)
        {
            ModifyTime = DateTime.Now;

            if (Document != null)
            {
                using FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                StreamWriter writer = Encoding != null ? new StreamWriter(fs, Encoding) : new StreamWriter(fs);
                Document.WriteTextTo(writer);
                writer.Flush();
            }
        }
    }
}