//#if NET472

using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace One.Core.Helper
{
    /// <summary>
    /// Windows服务帮助类
    /// </summary>
    public class ServiceHelper
    {
        #region C#控制服务

        /// <summary> C#判断服务是否存在 </summary>
        /// <param name="serviceName">  </param>
        /// <returns>  </returns>
        public static bool IsServiceExisted(string serviceName)
        {
            //logger.Trace("private bool IsServiceExisted(" + serviceName + ")");

            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController sc in services)
                {
                    if (sc.ServiceName.ToLower() == serviceName.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                Console.WriteLine("C#判断服务是否存在错误,请查看Error日志");


                return false;
            }
        }

        /// <summary> C#停止服务 </summary>
        /// <param name="serviceName">  </param>
        public static void ServiceStop(string serviceName)
        {
            //logger.Trace("private void ServiceStop(" + serviceName + ")");

            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController sc in services)
                {
                    if (sc.ServiceName.ToLower() == serviceName.ToLower())
                    {
                        if (sc.Status == ServiceControllerStatus.Running)
                        {
                            sc.Stop();
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("C#停止服务错误,请查看Error日志");

            }
        }

        /// <summary> C#检测服务状态 </summary>
        /// <param name="serviceName">  </param>
        /// <returns>  </returns>
        public static bool ServiceStat(string serviceName)
        {
            // logger.Trace("private bool ServiceStat(" + serviceName + ")");

            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController sc in services)
                {
                    if (sc.ServiceName.ToLower() == serviceName.ToLower())
                    {
                        if (sc.Status == ServiceControllerStatus.Running)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                Console.WriteLine("C#检测服务状态错误,请查看Error日志");

                return false;
            }
        }

        /// <summary> C#安装服务 </summary>
        /// <param name="serviceFilePath">  </param>
        public static void InstallService(string serviceFilePath)
        {
            // logger.Trace("private void InstallService(" + serviceFilePath + ")");

            try
            {
                using (AssemblyInstaller installer = new AssemblyInstaller())
                {
                    installer.UseNewContext = true;
                    installer.Path = serviceFilePath;
                    IDictionary savedState = new Hashtable();
                    installer.Install(savedState);
                    installer.Commit(savedState);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("C#安装服务错误,请查看Error日志");


            }
        }

        /// <summary> C#启动服务 </summary>
        /// <param name="serviceName">  </param>
        public static void ServiceStart(string serviceName)
        {
            // logger.Trace("private void ServiceStart(" + serviceName + ")");

            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController sc in services)
                {
                    if (sc.ServiceName.ToLower() == serviceName.ToLower())
                    {
                        if (sc.Status == ServiceControllerStatus.Stopped)
                        {
                            sc.Start();
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("C#启动服务错误,请查看Error日志");

            }
        }

        /// <summary> C#卸载服务 </summary>
        /// <param name="serviceFilePath">  </param>
        public static void UnstallService(string serviceFilePath)
        {
            //logger.Trace("private void UnstallService(" + serviceFilePath + ")");

            try
            {
                using (AssemblyInstaller installer = new AssemblyInstaller())
                {
                    installer.UseNewContext = true;
                    installer.Path = serviceFilePath;
                    installer.Uninstall(null);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("C#卸载服务错误,请查看Error日志");

            }
        }

        #endregion C#控制服务

        #region 系统命令控制服务

        /// <summary> 调用系统cmd关闭服务 </summary>
        /// <param name="Name">  </param>
        public static void NetStopService(string Name)
        {
            try
            {
                if (IsServiceExisted(Name))
                {
                    string AppName = "net";
                    string CmdPm = " stop " + Name;
                    Process MySqlProcess = new Process();
                    MySqlProcess.StartInfo.FileName = AppName;//设定需要执行的命令
                    MySqlProcess.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动
                    MySqlProcess.StartInfo.RedirectStandardInput = false;//不重定向输入
                    MySqlProcess.StartInfo.RedirectStandardOutput = true; //重定向输出
                    MySqlProcess.StartInfo.CreateNoWindow = true;//不创建窗口
                    MySqlProcess.StartInfo.Arguments = CmdPm;//启动参数
                    MySqlProcess.Start();
                    MySqlProcess.WaitForExit();
                    string RetOut = MySqlProcess.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("关闭服务出现错误!请查看Error日志.");

            }
        }

        /// <summary> 调用系统cmd删除服务 </summary>
        /// <param name="Name">  </param>
        public static void ScDellService(string Name)
        {
            //logger.Trace(System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                if (IsServiceExisted(Name))
                {
                    string AppName = "sc";
                    string CmdPm = " delete " + Name;

                    //logger.Trace(AppName + CmdPm);
                    Process MySqlProcess = new Process();
                    MySqlProcess.StartInfo.FileName = AppName;//设定需要执行的命令
                    MySqlProcess.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动
                    MySqlProcess.StartInfo.RedirectStandardInput = false;//不重定向输入
                    MySqlProcess.StartInfo.RedirectStandardOutput = true; //重定向输出
                    MySqlProcess.StartInfo.CreateNoWindow = true;//不创建窗口
                    MySqlProcess.StartInfo.Arguments = CmdPm;//启动参数
                    MySqlProcess.Start();
                    MySqlProcess.WaitForExit();
                    string RetOut = MySqlProcess.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("删除服务出现错误!请查看Error日志.");

            }
        }

        /// <summary> 调用系统cmd安装服务 </summary>
        /// <param name="AppName">  </param>
        /// <param name="IniFile">  </param>
        public static void CmdInstallService(string CmdPm, string AppName)
        {
            try
            {
                //MySql的安装 mysqld --install BioLabDB --defaults-file="C:\MySQL\my.ini"
                //string AppName = AppFile;
                //string CmdPm = " --install BioLabDB --defaults-file=\"" + IniFile + "\"";

                //logger.Info(CmdPm);
                Process MySqlProcess = new Process();
                MySqlProcess.StartInfo.FileName = AppName;//设定需要执行的命令
                MySqlProcess.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动
                MySqlProcess.StartInfo.RedirectStandardInput = false;//不重定向输入
                MySqlProcess.StartInfo.RedirectStandardOutput = true; //重定向输出
                MySqlProcess.StartInfo.CreateNoWindow = true;//不创建窗口
                MySqlProcess.StartInfo.Arguments = CmdPm;//启动参数
                MySqlProcess.Start();
                MySqlProcess.WaitForExit();
                string RetOut = MySqlProcess.StandardOutput.ReadToEnd();

                // logger.Info(AppName + CmdPm + "\r\n" + RetOut);
                if (!RetOut.Contains("Service successfully installed"))
                {
                    Console.WriteLine("安装过程完成,出现错误!请查看Error日志!");

                }
                else
                {
                    Console.WriteLine("安装成功!CMD返回" + RetOut);

                }
            }
            catch (Exception)
            {
                Console.WriteLine("安装错误!请查看Error日志!");

            }
        }

        /// <summary> 调用系统cmd启动服务 </summary>
        /// <param name="AppName">  </param>
        /// <param name="IniFile">  </param>
        public static void CmdStartService(string serviceName)
        {
            try
            {
                string AppName = "net";
                string CmdPm = " start " + serviceName;
                Process MySqlProcess = new Process();
                MySqlProcess.StartInfo.FileName = AppName;//设定需要执行的命令
                MySqlProcess.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动
                MySqlProcess.StartInfo.RedirectStandardInput = false;//不重定向输入
                MySqlProcess.StartInfo.RedirectStandardOutput = true; //重定向输出
                MySqlProcess.StartInfo.CreateNoWindow = true;//不创建窗口
                MySqlProcess.StartInfo.Arguments = CmdPm;//启动参数
                MySqlProcess.Start();
                MySqlProcess.WaitForExit();
                string RetOut = MySqlProcess.StandardOutput.ReadToEnd();

                Console.WriteLine(RetOut);

                //logger.Trace(AppName + CmdPm + "\r\n" + RetOut);

                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                Console.WriteLine("启动错误!请查看错误日志!");

            }
        }

        #endregion 系统命令控制服务
    }
}

//#endif