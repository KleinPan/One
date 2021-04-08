using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace One.Control.Helper
{
    /// <summary>
    /// 带颜色的消息帮助事件
    /// </summary>
   public class MessageHelper
    {
        public delegate void MessageEvent(string message, SolidColorBrush color);

        public static event MessageEvent NotifyMessage;

        /// <summary>
        /// 使用方法 Mes.NotifyMessage += SetNotifyMessage;  SetNotifyMessage(string message, SolidColorBrush fc)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public static void Notify(string message, SolidColorBrush color = null)
        {
            if (color == null)
            {
                color = Brushes.Black;
            }
            //NotifyMessage?.Invoke(DateTime.Now.ToString("MM月dd日 HH:mm:ss :\r\n") + message, color);
            NotifyMessage?.Invoke(message, color);

        }
    }
}
