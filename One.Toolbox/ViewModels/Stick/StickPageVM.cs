using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Stick;

using System.Collections.ObjectModel;

namespace One.Toolbox.ViewModels.Stick;

public partial class StickPageVM : BaseVM
{
    private const string configName = "sticks.json";

    /// <summary> 主要显示缩略图 </summary>
    public StickPageVM()
    {
        InitializeViewModel();
    }

    public override void InitializeViewModel()
    {
        base.InitializeViewModel();

        OcStickWindowList.Add(new StickItemVM(OcStickWindowList.Count));
    }

    public ObservableCollection<StickItemVM> OcStickWindowList { get; set; } = new ObservableCollection<StickItemVM>();

    [RelayCommand]
    private void Show()
    {
        ShowStickWindow(0);
    }

    private void ShowStickWindow(int index)
    {
        StickWindow target;
        if (OcStickWindowList.Count < index + 1)
        {
        }
        else
        {
            OcStickWindowList.ElementAt(index).Show();
        }
    }

    public void AddNewStick()
    {
        OcStickWindowList.Add(new StickItemVM(OcStickWindowList.Count));
    }

    [RelayCommand]
    private void Test()
    {
    }
}