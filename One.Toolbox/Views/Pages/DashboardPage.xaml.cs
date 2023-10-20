using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.Dashboard;

namespace One.Toolbox.Views.Pages;

public partial class DashboardPage
{
    public DashboardViewModel ViewModel { get; }

    public DashboardPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<DashboardViewModel>();

        InitializeComponent();
    }
}