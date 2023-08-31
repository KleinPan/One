using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

namespace One.Toolbox.Views.Pages
{
    /// <summary> 编码转换工具页面 </summary>
    public partial class StringConvertPage
    {
        public ViewModels.StringConvertViewModel ViewModel { get; }

        public StringConvertPage()
        {
            DataContext = ViewModel = App.Current.Services.GetService<StringConvertViewModel>();

            InitializeComponent();
        }
    }
}