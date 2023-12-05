using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.Stick;

namespace One.Toolbox.Views.Stick;

public partial class StickPage
{
    public StickPageVM ViewModel { get; }

    public StickPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<StickPageVM>();

        InitializeComponent();
    }
}