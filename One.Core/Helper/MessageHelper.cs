﻿
using System;
using System.Collections.Generic;
using System.Text;


namespace One.Core.Helper
{
  public  class MessageHelper
    {

        public delegate void MessageEvent(string message);

        public static event MessageEvent NotifyMessage;

        /// <summary>
        /// 使用方法 Mes.NotifyMessage += SetNotifyMessage;  SetNotifyMessage(string message, SolidColorBrush fc)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public static void Notify(string message)
        {
         
            //NotifyMessage?.Invoke(DateTime.Now.ToString("MM月dd日 HH:mm:ss :\r\n") + message, color);
            NotifyMessage?.Invoke(message);

        }
    }
}

