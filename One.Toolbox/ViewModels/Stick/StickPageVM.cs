using One.Core.Helpers;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Stick;

using System.Collections.ObjectModel;
using System.IO;

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

        var listFile = System.IO.Directory.GetFiles(One.Toolbox.Helpers.PathHelper.stickPath);

        for (int i = 0; i < listFile.Length; i++)
        {
            OcStickWindowList.Add(new StickItemVM(i)
            {
                ShowAddContent = false,
            });
        }

        OcStickWindowList.Add(new StickItemVM(OcStickWindowList.Count));
    }

    public ObservableCollection<StickItemVM> OcStickWindowList { get; set; } = new ObservableCollection<StickItemVM>();

    public void AddNewStick()
    {
        OcStickWindowList.Add(new StickItemVM(OcStickWindowList.Count));
    }
}