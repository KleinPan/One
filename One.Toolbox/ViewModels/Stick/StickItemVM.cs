using Microsoft.Extensions.DependencyInjection;

using One.Control.Controls.RichTextboxEx;
using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Stick;

using Org.BouncyCastle.Utilities.Encoders;

using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;

using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Vanara.PInvoke;

namespace One.Toolbox.ViewModels.Stick;

public partial class StickItemVM : BaseVM
{
    [ObservableProperty]
    private ImageSource screen;

    [ObservableProperty]
    private string screenPath;

    [ObservableProperty]
    private bool showAddContent = true;

    [ObservableProperty]
    private string buttonContent = "显示";

    public StickWindow StickWindow { get; set; }

    public int Index { get; set; }

    #region Prop

    [ObservableProperty]
    private StickType stickType;

    [ObservableProperty]
    private string stickName;

    [ObservableProperty]
    private string stickContentPath;

    [ObservableProperty]
    private int lineHeight = 1;

    [ObservableProperty]
    private ObservableCollection<StickTheme> themeList = new();

    [ObservableProperty]
    private StickTheme currentTheme;

    #endregion Prop

    private RichTextboxEx currentRtb;
    private System.Windows.Controls.Primitives.Popup popup;
    //private const string configName = "sticks.txt";

    //string tempPath;
    public StickItemVM(int index)
    {
        Index = index;
        InitializeViewModel();
    }

    public override void InitializeViewModel()
    {
        base.InitializeViewModel();

        ScreenPath = Helpers.PathHelper.tempPath + Index + ".png";

        StickName = "Temp_" + Index;

        ThemeList.Clear();
        ThemeList.Add(new StickTheme("#AFE958", "#C0ED7C"));//绿色
        ThemeList.Add(new StickTheme("#b8f7b0", "#c8f8c2"));//绿色

        ThemeList.Add(new StickTheme("#FEE65C", "#FEEC85"));//深黄色
        ThemeList.Add(new StickTheme("#FFEBAE", "#FFF1C6"));//浅黄色
        CurrentTheme = ThemeList[0];

        InitData();
    }

    private void UpdateImageSource(string path)
    {
        BitmapImage bi = new BitmapImage();

        bi.BeginInit();
        bi.UriSource = new Uri($"{path}", UriKind.Absolute);

        bi.EndInit();
        Screen = bi.Clone(); //完成图片的更新
        Screen.Freeze();
    }

    private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
    {
        BitmapImage bitmapImage = new BitmapImage();
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            bitmap.Save(ms, bitmap.RawFormat);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
        }
        return bitmapImage;
    }

    private void ScreenTheWindow()
    {
        System.Windows.Media.Imaging.RenderTargetBitmap targetBitmap = new System.Windows.Media.Imaging.RenderTargetBitmap((int)StickWindow.ActualWidth, (int)StickWindow.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);
        targetBitmap.Render(StickWindow);
        System.Windows.Media.Imaging.PngBitmapEncoder saveEncoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
        saveEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(targetBitmap));

        using (var stream = new MemoryStream())
        {
            saveEncoder.Save(stream);
            Screen = BitmapToBitmapImage(new Bitmap(stream));
        }
    }

    private void GetScreen(int index)
    {
    }

    #region CommonCommand

    [RelayCommand]
    public void Show()
    {
        if (StickWindow == null)
        {
            StickWindow = new StickWindow();
            StickWindow.DataContext = this;
            StickWindow.Show();

            ScreenTheWindow();
            ButtonContent = "隐藏";
        }
        else
        {
            ScreenTheWindow();
            StickWindow.Close();
            StickWindow = null;
            ButtonContent = "显示";
        }
    }

    [RelayCommand]
    private void AddNewStick()
    {
        StickName = "Temp";

        ShowAddContent = false;

        var service = App.Current.Services.GetService<StickPageVM>();
        service.AddNewStick();
    }

    [RelayCommand]
    private void InitRtbControl(object obj)
    {
        var a = obj as System.Windows.RoutedEventArgs;
        currentRtb = a.Source as RichTextboxEx;

        Task.Run(() =>
        {
            Task.Delay(100);
        });

        LoadXamlPackage(StickContentPath);
    }

    #endregion CommonCommand

    #region UpCommand

    [RelayCommand]
    private async Task RenameStick(object obj)
    {
        var dialog = await DialogHelper.Instance.ShowInteractiveDialog("请输入新标题！");
        if (dialog.Result == Dialogs.DialogResultEnum.OK)
        {
            StickName = dialog.InputText;
        }
    }

    /// <summary> 测试用 </summary>
    [RelayCommand]
    private void SearchContent()
    {
        //https://www.cnblogs.com/dreamos/p/12531366.html
        // currentRtb.HighLightSearch("aa");
    }

    [RelayCommand]
    private void ShowTheme(object obj)
    {
        popup = (System.Windows.Controls.Primitives.Popup)obj;

        popup.IsOpen = true;
    }

    [RelayCommand]
    private void SelectTheme(object obj)
    {
        var current = obj as Button;
        if (current != null)
        {
            StackPanel stackPanel = current.Parent as StackPanel;
            int index = stackPanel.Children.IndexOf(current);
            CurrentTheme = ThemeList.ElementAt(index);
        }

        if (popup != null)
        {
            popup.IsOpen = false;
        }
    }

    [RelayCommand]
    private void CloseWnd(object obj)
    {
        try
        {
            SaveData();
            var wnd = (Window)obj;
            wnd.Close();
        }
        catch (Exception)
        {
        }
    }

    [RelayCommand]
    private void DeactivatedWnd(object obj)
    {
        try
        {
            SaveData();
        }
        catch (Exception)
        {
        }
    }

    #region DataProcess

    void InitData()
    {
        try
        {
            var m = IOHelper.Instance.ReadContentFromLocal<StickM>(One.Toolbox.Helpers.PathHelper.dataPath + Index + ".data");

            CurrentTheme = m.CurrentTheme;
            StickName = m.StickName;
            StickType = m.StickType;
            StickContentPath = m.StickContentPath;
        }
        catch (Exception ex)
        {
        }
    }

    void LoadXamlPackage(string _fileName)
    {
        if (!File.Exists(_fileName))
        {
            return;
        }
        var ss = File.ReadAllBytes(_fileName);
        var a = XamlReader.Parse(Encoding.UTF8.GetString(ss));
        currentRtb.Document = (FlowDocument)a;
    }

    void SaveData()
    {
        StickContentPath = One.Toolbox.Helpers.PathHelper.dataPath + "aa";

        var m = ToModel();

        IOHelper.Instance.WriteContentTolocal(m, One.Toolbox.Helpers.PathHelper.dataPath + +Index + ".data");

        SaveXamlPackage(StickContentPath);
    }

    void SaveXamlPackage(string _fileName)
    {
        var ss = XamlWriter.Save(currentRtb.Document);
        File.WriteAllBytes(_fileName, Encoding.UTF8.GetBytes(ss));
    }

    #endregion DataProcess

    #endregion UpCommand

    #region BottomCommand

    /// <summary> 删除线 </summary>
    /// <param name="obj"> </param>
    [RelayCommand]
    private void ToggleStrikeThrough(object obj)
    {
        //var rtb = obj as RichTextBox;
        TextRange textRange = new TextRange(currentRtb.Selection.Start, currentRtb.Selection.End);

        TextDecorationCollection tdc =
                (TextDecorationCollection)currentRtb.
                     Selection.GetPropertyValue(Inline.TextDecorationsProperty);
        if (tdc == null || !tdc.Equals(TextDecorations.Strikethrough))
        {
            tdc = TextDecorations.Strikethrough;
        }
        else
        {
            tdc = new TextDecorationCollection();
        }
        textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, tdc);
    }

    #endregion BottomCommand

    #region CenterCommand

    [RelayCommand]
    private void InsertCheckbox(object obj)
    {
        var chb = new CheckBox() { Margin = new Thickness(0, 0, 5, 0) };

        Paragraph paragraph = new();

        // Create a new InlineUIContainer to contain the Button.
        InlineUIContainer myInlineUIContainer = new InlineUIContainer();

        // Set the BaselineAlignment property to "Bottom" so that the Button aligns properly with the text.
        myInlineUIContainer.BaselineAlignment = BaselineAlignment.Center;

        // Asign the button as the UI container's child.
        myInlineUIContainer.Child = chb;

        paragraph.Inlines.Add(myInlineUIContainer);

        currentRtb.Document.Blocks.Add(paragraph);
    }

    [RelayCommand]
    private void TestA(object obj)
    {
        var args = obj as MouseButtonEventArgs;
        if (args.Source is CheckBox chb)
        {
            var inlineUIContainer = chb.Parent as InlineUIContainer;
            var paragraph = inlineUIContainer.Parent as Paragraph;
            if (paragraph != null)
            {
                //paragraph.TextDecorations = TextDecorations.Strikethrough;
                var text = paragraph.Inlines.ElementAt(1) as Run;

                if ((bool)chb.IsChecked)//反着来
                {
                    text.TextDecorations = new TextDecorationCollection();
                }
                else
                {
                    text.TextDecorations = TextDecorations.Strikethrough;
                }
            }
        }
    }

    #endregion CenterCommand

    public StickM ToModel()
    {
        StickM stickM = new StickM();
        stickM.CurrentTheme = CurrentTheme;
        stickM.StickName = StickName;
        stickM.StickType = StickType;
        stickM.StickContentPath = StickContentPath;

        return stickM;
    }
}

public class StickM
{
    public StickTheme CurrentTheme { get; set; }
    public string StickName { get; set; }
    public StickType StickType { get; set; }

    public string StickContentPath { get; set; }
}