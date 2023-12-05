using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.Network;

namespace One.Toolbox.Views.Network;

public partial class NetworklPage
{
    public ViewModels.Network.NetworkVM ViewModel { get; }

    public NetworklPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<NetworkVM>();

        InitializeComponent();
    }
}