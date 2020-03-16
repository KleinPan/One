
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace One.Core.Helper
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                //throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
                //LogOutTxt("GetMD5HashFromFile() fail,error:" + ex.Message);


                Console.WriteLine("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 删除使用这个方法的exe,必须放在formclosed方法里边,并且调用后程序要退出
        /// </summary>
        public static void DeleteExe()
        {

            try
            {
                //LogOutTxt("CONDITION1 is defined");

                string cmd = "/C ping 1.1.1.1 -n 1 -w 1000 > Nul & Del " + "\"" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + "\"";

                //删除合成的文件
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", cmd);

                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.CreateNoWindow = true;

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;

                Process.Start(psi);


                //Application.Exit();
            }
            catch (Exception Exp)
            {
                Console.WriteLine(Exp.Message);

            }

        }


    }
}
