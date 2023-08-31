using Org.BouncyCastle.Asn1.X509.Qualified;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace One.Toolbox.ViewModels
{
    public partial class MainMenuItemViewModel : BaseViewModel
    {
        [ObservableProperty]
        private UserControl content;

        [ObservableProperty]
        private string header;

        [ObservableProperty]
        private Type? targetPageType;

        [ObservableProperty]
        private object icon;

        /// <summary> 若要将子元素停靠到另一个方向，必须将 属性设置为 LastChildFillfalse,并且还必须为最后一个子元素指定显式停靠方向。 </summary>
        [ObservableProperty]
        public Dock dock;

        public MainMenuItemViewModel()
        {
            //Content = App.GetService< Type.GetType(nameof(targetPageType)) >();
            Dock = Dock.Top;
            
        }

        public override string ToString()
        {
            return Header.ToString();
        }
    }
}