using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.Network;

namespace One.Toolbox.Views.Network;

public partial class NetworklPage
{
    public ViewModels.Network.NetworkPageVM ViewModel { get; }

    public NetworklPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<NetworkPageVM>();

        InitializeComponent();
    }
}