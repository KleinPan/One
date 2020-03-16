using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
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
        public static void CloseProc(string procName)
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
    }
}
