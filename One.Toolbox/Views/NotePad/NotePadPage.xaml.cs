using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.NotePad;

namespace One.Toolbox.Views.NotePad;

public partial class NotePadPage
{
    public NotePadPageVM ViewModel { get; }

    public NotePadPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<NotePadPageVM>();

        InitializeComponent();
    }
}