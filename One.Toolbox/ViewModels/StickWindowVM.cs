using One.Core.Helpers;
using One.Core.Helpers.EncryptionHelpers;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Media;

using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace One.Toolbox.ViewModels;

public enum StickType
{
    Common,
    Todo,
    Alert,
    Timeline,
}

public partial class StickWindowVM : BaseVM
{
    #region Prop

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

    private RichTextBox currentRtb;
    private System.Windows.Controls.Primitives.Popup popup;
    private const string configName = "sticks.txt";

    public StickWindowVM()
    {
        InitializeViewModel();

        isInitialized = true;

        StickName = "Test";

        themeList.Clear();
        themeList.Add(new StickTheme("#AFE958", "#C0ED7C"));//绿色
        themeList.Add(new StickTheme("#b8f7b0", "#c8f8c2"));//绿色

        themeList.Add(new StickTheme("#FEE65C", "#FEEC85"));//深黄色
        themeList.Add(new StickTheme("#FFEBAE", "#FFF1C6"));//浅黄色
        CurrentTheme = themeList[0];

        InitData();
    }

    #region CommonCommand

    [RelayCommand]
    private void InitRtbControl(object obj)
    {
        var a = obj as System.Windows.RoutedEventArgs;
        currentRtb = a.Source as RichTextBox;
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
        if (stickContent != null)
        {
        }
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

    void InitData()
    {
        try
        {
            var m = IOHelper.Instance.ReadContentFromLocal<StickM>(One.Toolbox.Helpers.PathHelper.dataPath + configName);

            CurrentTheme = m.CurrentTheme;
            StickName = m.StickName;
            StickType = m.StickType;
            StickContent = m.StickContent;
        }
        catch (Exception ex)
        {
        }
    }

    void SaveData()
    {
        var m = ToModel();
        IOHelper.Instance.WriteContentTolocal(m, One.Toolbox.Helpers.PathHelper.dataPath + configName);
    }

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

    public StickM ToModel()
    {
        StickM stickM = new StickM();
        stickM.CurrentTheme = CurrentTheme;
        stickM.StickName = StickName;
        stickM.StickType = StickType;
        stickM.StickContent = StickContent;

        return stickM;
    }
}

public partial class StickTheme : ObservableObject
{
    [ObservableProperty]
    private System.Windows.Media.Brush headerBrush;

    [ObservableProperty]
    private System.Windows.Media.Brush backBrush;

    public StickTheme()
    {
    }

    public StickTheme(string head, string back)
    {
        HeaderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(head));
        BackBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(back));
    }
}

public class StickM
{
    public StickTheme CurrentTheme { get; set; }
    public string StickName { get; set; }
    public StickType StickType { get; set; }

    public string StickContent { get; set; }
}