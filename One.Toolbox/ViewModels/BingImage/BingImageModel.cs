using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace One.Toolbox.ViewModels.BingImage
{
    public partial class UsefullImageInfo : ObservableObject
    {
        /// <summary> </summary>
        /// <param name="uAction">  指定要设置的参数。参考uAction常数表。 </param>
        /// <param name="uParam">   参考uAction常数表。 </param>
        /// <param name="lpvParam"> 按引用调用的Integer、Long和数据结构。 </param>
        /// <param name="fuWinIni"> 这个参数规定了在设置系统参数的时候，是否应更新用户设置参数。 </param>
        /// <returns> </returns>
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
                    int uAction,
                    int uParam,
                    string lpvParam,
                    int fuWinIni
                );

        public static void SetImageToDesktop(string filePath)
        {
            SystemParametersInfo(20, 1, filePath, 1);
        }

        [ObservableProperty]
        private string downloadUrl;

        [ObservableProperty]
        private string copyright;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string localImageName;

        [ObservableProperty]
        private string localImagePath;

        [RelayCommand]
        private void Set()
        {
            SetImageToDesktop(LocalImagePath);
        }
    }

    internal class BingImageModel
    {
        public List<BingImage> images { get; set; }
        public Tooltips tooltips { get; set; }
    }

    public class Tooltips
    {
        public string loading { get; set; }
        public string previous { get; set; }
        public string next { get; set; }
        public string walle { get; set; }
        public string walls { get; set; }
    }

    public class BingImage
    {
        public string startdate { get; set; }
        public string fullstartdate { get; set; }
        public string enddate { get; set; }
        public string url { get; set; }
        public string urlbase { get; set; }
        public string copyright { get; set; }
        public string copyrightlink { get; set; }
        public string title { get; set; }
        public string quiz { get; set; }
        public bool wp { get; set; }
        public string hsh { get; set; }
        public int drk { get; set; }
        public int top { get; set; }
        public int bot { get; set; }
        public object[] hs { get; set; }
    }
}