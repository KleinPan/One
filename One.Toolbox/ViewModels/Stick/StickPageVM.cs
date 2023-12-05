using One.Toolbox.ViewModels.Base;

namespace One.Toolbox.ViewModels.Stick;

public partial class StickPageVM : BaseVM
{
    private const string configName = "sticks.txt";

    public StickPageVM()
    {
        InitializeViewModel();

        isInitialized = true;
    }
}