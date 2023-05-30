namespace One.Toolbox.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        public static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();

        protected bool isInitialized = false;

        /// <summary> 进入当前页面 </summary>
        public virtual void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();
        }

        /// <summary> 从当前页面离开 </summary>
        public virtual void OnNavigatedFrom()
        {
        }

        public virtual void InitializeViewModel()
        {
            isInitialized = true;
        }
    }
}