// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;

using Microsoft.Win32;

using One.Toolbox.ViewModels.Base;

using Org.BouncyCastle.Utilities;

using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace One.Toolbox.ViewModels;

public partial class NotePadViewModel : BaseViewModel
{
    [ObservableProperty]
    private TextDocument document;

    [ObservableProperty]
    private bool isDirty;

    [ObservableProperty]
    private bool isReadOnly;

    [ObservableProperty]
    public string isReadOnlyReason;

    /// <summary> 当前打开的文件路径 </summary>
    [ObservableProperty]
    public string filePath;

    /// <summary> AvalonEdit exposes a Highlighting property that controls whether keywords, comments and other interesting text parts are colored or highlighted in any other visual way. This property exposes the highlighting information for the text file managed in this viewmodel class. </summary>
    [ObservableProperty]
    private IHighlightingDefinition highlightingDefinition;

    [ObservableProperty]
    private Encoding encoding = Encoding.UTF8;

    public NotePadViewModel()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    void InitData()
    {
        document = new TextDocument();
    }

    [RelayCommand]
    private void OpenFile()
    {
        var dlg = new OpenFileDialog();
        if (dlg.ShowDialog().GetValueOrDefault())
        {
            var fileViewModel = LoadDocument(dlg.FileName);
        }
    }

    [RelayCommand]
    private void SaveFile()
    {
        SaveDocument();
    }

    bool LoadDocument(string filePath)
    {
        if (File.Exists(filePath))
        {
            var hlManager = HighlightingManager.Instance;

            Document = new TextDocument();
            string extension = System.IO.Path.GetExtension(filePath);
            HighlightingDefinition = hlManager.GetDefinitionByExtension(extension);

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
                using (StreamReader reader = FileReader.OpenStream(fs, Encoding))
                {
                    Document = new TextDocument(reader.ReadToEnd());
                }
            }

            FilePath = filePath;

            return true;
        }

        return false;
    }

    void SaveDocument()
    {
        if (FilePath == null)
            throw new ArgumentNullException("fileName");
        using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            StreamWriter writer = Encoding != null ? new StreamWriter(fs, Encoding) : new StreamWriter(fs);
            if (Document != null)
                Document.WriteTextTo(writer);
            writer.Flush();
        }
    }
}