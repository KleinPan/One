using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.FileMonitor;

using System.Windows.Controls;

namespace One.Toolbox.Views.FileMonitor
{
    public partial class FileMonitorPage : UserControl
    {
        public FileMonitorPage()
        {
            DataContext = App.Current.Services.GetService<FileMonitorPageVM>();
            InitializeComponent();
        }
    }
}