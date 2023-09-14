
using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

namespace One.Toolbox.Views.NotePad;

public partial class NotePadPage
{
    public NotePadViewModel ViewModel { get; }

    public NotePadPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<NotePadViewModel>();

        InitializeComponent();
    }
}