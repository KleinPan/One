namespace One.Toolbox.ViewModels.Base
{
    public class BaseViewModel : ObservableObject
    {
        public static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();

        protected bool isInitialized = false;

        /// <summary> 进入当前页面 </summary>
        public virtual void OnNavigatedEnter()
        {
            if (!isInitialized)
                InitializeViewModel();
        }

        /// <summary> 从当前页面离开 </summary>
        public virtual void OnNavigatedLeave()
        {
        }

        public virtual void InitializeViewModel()
        {
            isInitialized = true;
        }

        public virtual void WriteTraceLog(string msg)
        {
            NLogger.Debug(msg);
        }

        public virtual void WriteDebugLog(string msg)
        {
            NLogger.Debug(msg);
        }

        public virtual void WriteInfoLog(string msg)
        {
            NLogger.Info(msg);
        }
    }
}