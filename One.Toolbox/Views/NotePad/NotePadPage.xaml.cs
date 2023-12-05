using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.NotePad;

namespace One.Toolbox.Views.NotePad;

public partial class NotePadPage
{
    public NotePadVM ViewModel { get; }

    public NotePadPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<NotePadVM>();

        InitializeComponent();
    }
}