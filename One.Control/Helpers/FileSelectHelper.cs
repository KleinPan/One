using System.Collections.Generic;
using System.Windows;

using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace One.Control.Helpers
{
    public class FileSelectHelper
    {
        //public static string SetInitDir { get; set; } = PathConfig.ConfigPath;

        /// <summary> 打开自定义标题的文件选择对话框 </summary>
        /// <param name="title">   </param>
        /// <param name="fileExt"> XML|raw*.xml </param>
        /// <returns> </returns>
        public static string OpenFileDialog(string title = "", string fileExt = "")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//允许打开多个文件
            //dialog.InitialDirectory = SetInitDir;
            if (!string.IsNullOrEmpty(title))
            {
                dialog.Title = title;
            }

            //dialog.DefaultExt = fileExt;//打开文件时显示的可选文件类型
            if (!string.IsNullOrEmpty(fileExt))
            {
                dialog.Filter = $"{fileExt}|All Files|*.*";//打开单个文件
            }
            else
            {
                dialog.Filter = $"All Files|*.*";//打开单个文件
            }

            if (dialog.ShowDialog() == true)
                return dialog.FileNames[0];
            else
            {
                MessageBox.Show("File path error, please check!");
                return null;
            }
        }

        public static List<string> OpenMultiFileDialog(string title = "", string fileExt = "")
        {
            List<string> vs = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//允许打开多个文件
            //dialog.InitialDirectory = SetInitDir;
            if (!string.IsNullOrEmpty(title))
            {
                dialog.Title = title;
            }

            //dialog.Filter = fileExt + "xlsx文件|" + "*." + fileExt + "|xls文件|*.xls";//打开多个文件
            if (!string.IsNullOrEmpty(fileExt))
            {
                dialog.Filter = $"{fileExt}|All Files|*.*";//打开单个文件
            }
            else
            {
                dialog.Filter = $"All Files|*.*";//打开单个文件
            }
            if (dialog.ShowDialog() == true)
            {
                foreach (var item in dialog.FileNames)
                {
                    vs.Add(item);
                }
            }
            else
            {
                MessageBox.Show("File path error, please check!");
            }
            return vs;
        }
    }
}