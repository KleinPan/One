using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels.BingImage;
using One.Toolbox.ViewModels.LotteryDraw;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace One.Toolbox.Views.LotteryDraw
{
    /// <summary> LotteryDrawPage.xaml 的交互逻辑 </summary>
    public partial class LotteryDrawPage : UserControl
    {
        public LotteryDrawPage()
        {
            DataContext = App.Current.Services.GetService<LotteryDrawViewModel>();
            InitializeComponent();
        }
    }
}