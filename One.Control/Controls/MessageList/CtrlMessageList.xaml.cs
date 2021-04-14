using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace One.Controls.Controls.MessageList
{
    /// <summary> 显示消息类 </summary>

    public class MessageShowed
    {
        public SolidColorBrush TimeForeground { get; set; }
        public string time { get; set; }

        public SolidColorBrush MessageForeground { get; set; }
        public string message { get; set; }
    }

    /// <summary> MessageList.xaml 的交互逻辑 </summary>
    
    public partial class CtrlMessageList : UserControl
    {
        // private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public CtrlMessageList()
        {
            InitializeComponent();

            // Mes.NotifyMessage += SetNotifyMessage;

            //初始化配置日志
            //LogManager.Configuration = new XmlLoggingConfiguration(AppDomain.CurrentDomain.BaseDirectory.ToString() + "/NLogLib.config");
        }

        /// <summary> 设置Nlog的配置文件路径 </summary>
        /// <param name="path">  </param>
        public void SetLogConfigPath(string path)
        {
            //初始化配置日志
            // LogManager.Configuration = new XmlLoggingConfiguration(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
        }

        #region 消息显示

        /// <summary> 显示用 </summary>
        /// <param name="message"> 显示的 </param>
        /// <param name="fc">      前景色 </param>
        private void SetNotifyMessage(string message, SolidColorBrush fc)
        {
            string now = DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                string str_show_message = string.Format("{0}", message);
                var newitem = new MessageShowed()
                {
                    time = now,
                    TimeForeground = System.Windows.Media.Brushes.Gray,
                    message = str_show_message,
                    MessageForeground = fc,
                };

                lvwMes.Items.Insert(lvwMes.Items.Count, newitem);
                //Log.WriteLog(str_show_message);
                //logger.Trace(str_show_message);

                lvwMes.ScrollIntoView(newitem);
                if (lvwMes.Items.Count > 100)
                {
                    lvwMes.Items.RemoveAt(0);
                }
            });
        }

        private static string GetDescription(Enum en)
        {
            Type type = en.GetType();   //获取类型
            MemberInfo[] memberInfos = type.GetMember(en.ToString());   //获取成员
            if (memberInfos != null && memberInfos.Length > 0)
            {
                DescriptionAttribute[] attrs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];   //获取描述特性

                if (attrs != null && attrs.Length > 0)
                {
                    return attrs[0].Description;    //返回当前描述
                }
            }
            return en.ToString();
        }

        #endregion 消息显示

        private void list_log_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                try
                {
                    var text = lvwMes.SelectedItem as MessageShowed;
                    string s = text.message;

                    Clipboard.SetText(s);
                    //lvwMes.Items.Clear();
                }
                catch (Exception)
                {
                }
            });
        }
    }
}