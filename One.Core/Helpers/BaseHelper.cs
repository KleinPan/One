using System;

namespace One.Core.Helpers
{
    /// <summary> 帮助类的基类 </summary>
    public class BaseHelper
    {
        private Action<string> _logAction;

        public BaseHelper(Action<string> logAction = null)
        {
            _logAction = logAction;
        }

        /// <summary> 基类实现的打印log </summary>
        /// <param name="msg"> </param>
        public virtual void WriteLog(string msg)
        {
            _logAction?.Invoke(msg);
        }
    }
}