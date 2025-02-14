using Microsoft.Extensions.DependencyInjection;

using One.Base.Helpers;
using One.Control.Controls.RichTextboxEx;
using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Stick;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;

using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace One.Toolbox.ViewModels.Stick;

public partial class StickItemVM : BaseVM
{
    public StickWindow StickWindow { get; set; }

    public int Index { get; set; }

    partial void OnDefaultOnChanged(bool value)
    {
        SaveSetting();
    }

    #region Prop

    [ObservableProperty]
    private bool defaultOn;

    /// <summary> 预览 </summary>
    [ObservableProperty]
    private ImageSource screen;

    [ObservableProperty]
    private bool showAddContent = true;

    [ObservableProperty]
    private string buttonContent = "显示";

    [ObservableProperty]
    private StickType stickType;

    [ObservableProperty]
    private string stickName;

    [ObservableProperty]
    private string stickContent;

    [ObservableProperty]
    private int lineHeight = 1;

    [ObservableProperty]
    private ObservableCollection<StickTheme> themeList = new();

    [ObservableProperty]
    private StickTheme currentTheme;

    #endregion Prop

    private RichTextboxEx currentRtb;
    private System.Windows.Controls.Primitives.Popup popup;

    public StickItemVM(int index)
    {
        Index = index;
        InitializeViewModel();
    }

    public override void InitializeViewModel()
    {
        base.InitializeViewModel();

        StickName = "Temp";

        ThemeList.Clear();
        ThemeList.Add(new StickTheme("#AFE958", "#C0ED7C"));//绿色
        ThemeList.Add(new StickTheme("#b8f7b0", "#c8f8c2"));//绿色

        ThemeList.Add(new StickTheme("#FEE65C", "#FEEC85"));//深黄色
        ThemeList.Add(new StickTheme("#FFEBAE", "#FFF1C6"));//浅黄色
        CurrentTheme = ThemeList[0];

        InitData();

        if (DefaultOn)
        {
            Show();
        }

        //WeakReferenceMessenger.Default.Register<CloseMessage>(this, (r, m) =>
        //{
        //    SaveSetting();
        //});
    }

    private static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
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
        RenderTargetBitmap targetBitmap = new RenderTargetBitmap((int)StickWindow.ActualWidth, (int)StickWindow.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);

        targetBitmap.Render(StickWindow);
        System.Windows.Media.Imaging.PngBitmapEncoder saveEncoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
        saveEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(targetBitmap));

        using (var stream = new MemoryStream())
        {
            saveEncoder.Save(stream);
            Screen = BitmapToBitmapImage(new Bitmap(stream));
        }
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

        LoadXamlPackage(StickContent);
    }

    #endregion CommonCommand

    #region UpCommand

    private Window MyWindow;

    [RelayCommand]
    private void Loaded(object obj)
    {
        MyWindow = obj as Window;
    }

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
        var button = obj as Button;
        if (button != null)
        {
            var temp = button.DataContext as StickTheme;
            CurrentTheme = temp;
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
            SaveSetting();
            var rootWnd = (Window)obj;
            rootWnd.Close();
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
            Debug.WriteLine("DeactivatedWnd");
            SaveSetting();
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
            var m = IOHelper.Instance.ReadContentFromLocal<StickItemM>(One.Toolbox.Helpers.PathHelper.stickPath + "Stick" + Index + ".data");

            CurrentTheme = m.CurrentTheme;
            StickName = m.StickName;
            StickType = m.StickType;
            StickContent = m.StickContent;
            DefaultOn = m.DefaultOn;
        }
        catch (Exception ex)
        {
        }
    }

    void LoadXamlPackage(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        var a = XamlReader.Parse(data);
        currentRtb.Document = (FlowDocument)a;
    }

    void SaveSetting()
    {
        if (currentRtb != null)
        {
            StickContent = XamlWriter.Save(currentRtb.Document);
        }

        var m = ToModel();

        IOHelper.Instance.WriteContentTolocal(m, One.Toolbox.Helpers.PathHelper.stickPath + "Stick" + Index + ".data");
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

    public StickItemM ToModel()
    {
        StickItemM stickM = new();
        stickM.CurrentTheme = CurrentTheme;
        stickM.StickName = StickName;
        stickM.StickType = StickType;
        stickM.StickContent = StickContent;
        stickM.DefaultOn = DefaultOn;

        return stickM;
    }
}

public class StickItemM
{
    public StickTheme CurrentTheme { get; set; }
    public string StickName { get; set; }
    public StickType StickType { get; set; }
    public bool DefaultOn { get; set; }

    public string StickContent { get; set; }
}