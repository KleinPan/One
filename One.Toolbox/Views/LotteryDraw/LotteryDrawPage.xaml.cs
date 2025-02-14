using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.LotteryDraw;

using System.Windows.Controls;

namespace One.Toolbox.Views.LotteryDraw
{
    /// <summary>LotteryDrawPage.xaml 的交互逻辑</summary>
    public partial class LotteryDrawPage : UserControl
    {
        public LotteryDrawPage()
        {
            DataContext = App.Current.Services.GetService<LotteryDrawPageVM>();
            InitializeComponent();
        }
    }
}