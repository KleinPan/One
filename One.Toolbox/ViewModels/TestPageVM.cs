using One.Toolbox.ViewModels.Base;

namespace One.Toolbox.ViewModels;

public partial class TestPageVM : BaseVM
{
    public TestPageVM()
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