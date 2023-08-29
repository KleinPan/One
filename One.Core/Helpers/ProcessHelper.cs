using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace One.Core.Helpers
{
    public class ProcessHelper : BaseHelper
    {
        public Action<object> logAction;
        public Process process;

        public ProcessHelper(Action<object> LogDelegate) : base(LogDelegate)
        {
            logAction = LogDelegate;
        }

        public void RunExe(string exeWorkDirectory, string exeName, string parmams)
        {
            if (process == null)
            {
                process = new Process();
            }

            WriteLog("Excutdir:" + exeWorkDirectory + exeName + ",Param: " + parmams);
            process.StartInfo.WorkingDirectory = exeWorkDirectory;
            process.StartInfo.FileName = exeWorkDirectory + exeName;
            process.StartInfo.Arguments = parmams;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            //return process.ExitCode == 0;
        }

        public bool RunExeAndReadResult(string exeWorkDirectory, string exeName, string parmams, out string res)
        {
            string commandLineString = "";

            if (process == null)
            {
                process = new Process();
            }
            res = "";
            WriteLog("Excutdir:" + exeWorkDirectory + exeName + ",Param: " + parmams);
            process.StartInfo.WorkingDirectory = exeWorkDirectory;
            process.StartInfo.FileName = exeWorkDirectory + exeName;
            process.StartInfo.Arguments = parmams;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            while (!process.HasExited)
            {
                commandLineString = process.StandardOutput.ReadLine();
                WriteLog(commandLineString);
                res += commandLineString;
            }

            commandLineString = process.StandardOutput.ReadToEnd();
            res += commandLineString;
            WriteLog("ReadToEnd = " + commandLineString);
            commandLineString = process.StandardError.ReadToEnd();
            res += commandLineString;
            WriteLog("StandardError = " + commandLineString + ".");

            process.WaitForExit(1000);
            WriteLog("ExecCMD ... exit code is ..." + process.ExitCode.ToString());
            return process.ExitCode == 0;
        }

        /// <summary> 读输出流，3秒内要做完，不然杀进程 </summary>
        /// <param name="exeWorkDirectory"> </param>
        /// <param name="exeName">          </param>
        /// <param name="parmams">          </param>
        /// <param name="res">              </param>
        /// <param name="second">           </param>
        /// <returns> </returns>
        public bool RunExeAndReadResultWithTimeLimit(string exeWorkDirectory, string exeName, string parmams, out string res, int second = 3)
        {
            WriteLog("RunExeAndReadResultWithTimeLimit");
            string commandLineString = "";

            res = "";
            process = new Process();
            WriteLog("WorkingDirectory:" + exeWorkDirectory + ",FileName:" + exeName + ",Param: " + parmams);
            process.StartInfo.WorkingDirectory = exeWorkDirectory;
            process.StartInfo.FileName = exeName;
            process.StartInfo.Arguments = parmams;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                WriteLog("ERROR Start");
                WriteLog($"ErrorDataReceived={e.Data}");
                WriteLog("ERROR End");
            });

            process.Start();

            try
            {
                Debug.WriteLine($"Process ID={process.Id}");

                bool check = true;
                Task.Run(() =>
                {
                    DateTime start = DateTime.Now;
                    DateTime end = start;

                    while (true)
                    {
                        end = DateTime.Now;
                        Thread.Sleep(1000);
                        bool bFlag = (end - start) < TimeSpan.FromSeconds(second);

                        if (check)
                        {
                            if (!bFlag)
                            {
                                process.Kill();

                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                });
                while (!process.HasExited)
                {
                    commandLineString = process.StandardOutput.ReadLine();

                    res += commandLineString;

                    WriteLog($"Process ReadLine={res}");
                }

                commandLineString = process.StandardOutput.ReadToEnd();
                res += commandLineString;
                WriteLog($"Read End={res}");
                process.StandardOutput.Close();
                Debug.WriteLine($"Process ID = {process.Id} StandardOutput.Close()");

                process.WaitForExit();
                check = false;
                if (process.ExitCode != 0)
                {
                    int code = process.ExitCode;
                    process.Kill();

                    return false;
                }
                else
                {
                    //process.Close();
                    //process.Kill();
                    process.Dispose();

                    Thread.Sleep(100);

                    return true;
                }
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        #region Kill

        public static void KillProcessByName(string processName)
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
                        thisproc.WaitForExit((int)TimeSpan.FromSeconds(10)
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

        public static void KillProcessByID(int processID)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程
            try
            {
                var thisproc = Process.GetProcessById(processID);
                if (thisproc.CloseMainWindow())
                {
                    thisproc.WaitForExit((int)TimeSpan.FromSeconds(10)
                        .TotalMilliseconds); //give some time to process message
                }

                if (!thisproc.HasExited)
                {
                    thisproc.Kill(); //TODO show UI message asking user to close program himself instead of silently killing it
                }

                Console.WriteLine("杀死" + processID + "成功！");
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine("杀死" + processID + "失败！");
            }
        }

        #endregion Kill
    }
}