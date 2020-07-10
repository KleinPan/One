using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace One.Core.Helper
{
    /// <summary>
    /// 进程帮助类
    /// </summary>
    public class ProcessHelper
    {
        public static void KillProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程
            try
            {
                // var a = Process.GetProcessesByName(processName);
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    //if (!thisproc.CloseMainWindow())
                    //{
                    //    thisproc.Kill();
                    //}

                    if (thisproc.CloseMainWindow())
                    {
                        thisproc.WaitForExit((int) TimeSpan.FromSeconds(10)
                            .TotalMilliseconds); //give some time to process message
                    }

                    if (!thisproc.HasExited)
                    {
                        thisproc.Kill(); //TODO show UI message asking user to close program himself instead of silently killing it
                    }

                    Console.WriteLine("杀死" + processName + "成功！");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine("杀死" + processName + "失败！");
            }
        }

        /// <summary>
        /// 启动、结束进程测试程序
        /// </summary>
        /// <param name="procName">进程名称</param>
        public static void TestProc(string procName)
        {
            try
            {
                Process myProcess;
                myProcess = Process.Start(procName);
                // Display physical memory usage 5 times at intervals of 2 seconds.
                for (int i = 0; i < 5; i++)
                {
                    if (!myProcess.HasExited)
                    {
                        // Discard cached information about the process.
                        myProcess.Refresh();
                        // Print working set to console.
                        Console.WriteLine("Physical Memory Usage: " + myProcess.WorkingSet64.ToString());

                        // Wait 2 seconds.
                    }
                    else
                    {
                        break;
                    }
                }

                // Close process by sending a close message to its main window.
                myProcess.CloseMainWindow();
                // Free resources associated with process.
                myProcess.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The following exception was raised: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary> 运行一个控制台程序并返回其输出参数。 </summary>
        /// <param name="filename">  方法名 </param>
        /// <param name="arguments"> </param>
        /// <param name="recordLog"> 是否记录日志</param>
        /// <returns> </returns>
        public static string RunApp(string filename, string arguments, bool recordLog)
        {
            try
            {
                if (recordLog)
                {
                    Trace.WriteLine(filename + " " + arguments);
                }

                Process proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (System.IO.StreamReader sr =
                    new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    //string txt = sr.ReadToEnd();
                    //sr.Close();
                    //if (recordLog)
                    //{
                    //    Trace.WriteLine(txt);
                    //}
                    //if (!proc.HasExited)
                    //{
                    //    proc.Kill();
                    //}
                    //上面标记的是原文，下面是我自己调试错误后自行修改的
                    Thread.Sleep(100); //貌似调用系统的nslookup还未返回数据或者数据未编码完成，程序就已经跳过直接执行
                    //txt = sr.ReadToEnd()了，导致返回的数据为空，故睡眠令硬件反应
                    if (!proc.HasExited) //在无参数调用nslookup后，可以继续输入命令继续操作，如果进程未停止就直接执行
                    {
                        //txt = sr.ReadToEnd()程序就在等待输入，而且又无法输入，直接掐住无法继续运行
                        proc.Kill();
                    }

                    string txt = sr.ReadToEnd();
                    sr.Close();
                    if (recordLog)
                        Trace.WriteLine(txt);
                    return txt;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.Message;
            }
        }
    }
}