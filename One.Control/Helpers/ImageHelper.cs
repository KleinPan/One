using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace One.Control.Helpers
{
    public class ImageHelper
    {
        /// <summary> Assembly: PresentationCore.dll </summary>
        /// <param name="imageName"> </param>
        /// <returns> </returns>
        public static BitmapImage FindImage(string imageName, string fullDir)
        {
            string path = fullDir + "\\" + imageName;

            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();  //给BitmapImage对象赋予数据的时候，需要用BeginInit()开始，用EndInit()结束
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.DecodePixelWidth = 1080;   //对大图片，可以节省内存。尽可能不要同时设置DecodePixelWidth和DecodePixelHeight，否则宽高比可能改变
            bitmapImage.EndInit();

            return bitmapImage.Clone();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }
    }
}