using One.Toolbox.ViewModels.Base;

namespace One.Toolbox.ViewModels;

public partial class CopyViewModel : BaseVM
{
    public CopyViewModel()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    void InitData()
    {
    }
}