using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.DataProcess;

namespace One.Toolbox.Views.DataProcess
{
    /// <summary> 编码转换工具页面 </summary>
    public partial class StringConvertPage
    {
        public ViewModels.DataProcess.StringConvertVM ViewModel { get; }

        public StringConvertPage()
        {
            DataContext = ViewModel = App.Current.Services.GetService<StringConvertVM>();

            InitializeComponent();
        }
    }
}