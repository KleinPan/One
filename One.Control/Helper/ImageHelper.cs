using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace One.Control.Helper
{
    public class ImageHelper
    {
        /// <summary>
        /// Assembly: PresentationCore.dll
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static BitmapImage FindImage(string imageName,string dir= @"\Resources\")
        {
            string path = System.IO.Directory.GetCurrentDirectory() + dir + imageName;

            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();  //给BitmapImage对象赋予数据的时候，需要用BeginInit()开始，用EndInit()结束
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.DecodePixelWidth = 1080;   //对大图片，可以节省内存。尽可能不要同时设置DecodePixelWidth和DecodePixelHeight，否则宽高比可能改变
            bitmapImage.EndInit();

            return bitmapImage.Clone();
        }
    }
}
