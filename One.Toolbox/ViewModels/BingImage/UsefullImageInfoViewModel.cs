using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace One.Toolbox.ViewModels.BingImage
{
    public partial class UsefullImageInfoViewModel : ObservableObject
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

        public UsefullImageInfoModel ToModel()
        {
            UsefullImageInfoModel usefullImageInfoViewModel = new UsefullImageInfoModel();
            usefullImageInfoViewModel.Title = Title;
            usefullImageInfoViewModel.Copyright = Copyright;
            usefullImageInfoViewModel.LocalImagePath = LocalImagePath;
            usefullImageInfoViewModel.LocalImageName = LocalImageName;
            usefullImageInfoViewModel.DownloadUrl = DownloadUrl;

            return usefullImageInfoViewModel;
        }
    }

    public class UsefullImageInfoModel
    {
        public string DownloadUrl;

        public string Copyright;

        public string Title;

        public string LocalImageName;

        public string LocalImagePath;

        public UsefullImageInfoViewModel ToVM()
        {
            UsefullImageInfoViewModel usefullImageInfoViewModel = new UsefullImageInfoViewModel();
            usefullImageInfoViewModel.Title = Title;
            usefullImageInfoViewModel.Copyright = Copyright;
            usefullImageInfoViewModel.LocalImagePath = LocalImagePath;
            usefullImageInfoViewModel.LocalImageName = LocalImageName;
            usefullImageInfoViewModel.DownloadUrl = DownloadUrl;

            return usefullImageInfoViewModel;
        }
    }
}