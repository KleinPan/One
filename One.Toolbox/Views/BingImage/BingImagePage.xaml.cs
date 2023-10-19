using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.BingImage;

namespace One.Toolbox.Views.BingImage;

public partial class BingImagePage
{
    public BingImageViewModel ViewModel { get; }

    public BingImagePage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<BingImageViewModel>();

        InitializeComponent();
    }
}