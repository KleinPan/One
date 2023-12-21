using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

using System.Windows.Controls;

namespace One.Toolbox.Views
{
    public partial class TestPage : UserControl
    {
        public TestPage()
        {
            DataContext = App.Current.Services.GetService<TestPageVM>();
            InitializeComponent();
        }
    }
}