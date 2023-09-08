using One.Toolbox.Helpers;
using One.Toolbox.Models;
using One.Toolbox.Tools;

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace One.Toolbox.Component
{
    /// <summary> 来什么显示什么，不做处理 </summary>
    public class FlowDocumentComponent
    {
        public static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();
        private FlowDocumentScrollViewer FlowDocumentScrollViewer { get; set; }

        /// <summary> 数据缓冲 </summary>
        private List<DataShowCommon> DataQueue = new List<DataShowCommon>();

        /// <summary> 消息来的信号量 </summary>
        private EventWaitHandle waitQueue = new AutoResetEvent(false);

        public static string packLengthWarn = "";
        private static string uiTooLag = "";
        private static string packsTooMuch = "";
        private static string logAutoClearWarn = "";
        private ScrollViewer sv;

        /// <summary> 禁止自动滚动？ </summary>
        public bool LockLog { get; set; } = false;

        public FlowDocumentComponent(FlowDocumentScrollViewer flowDocumentScrollViewer)
        {
            FlowDocumentScrollViewer = flowDocumentScrollViewer;
            sv = FlowDocumentScrollViewer.Template.FindName("PART_ContentHost", FlowDocumentScrollViewer) as ScrollViewer;
            sv.CanContentScroll = true;

            packLengthWarn = ResourceHelper.FindStringResource("SettingMaxShowPackWarn");
            logAutoClearWarn = ResourceHelper.FindStringResource("SettingMaxPacksWarn");
            packsTooMuch = ResourceHelper.FindStringResource("BuffPacksTooMuchWarn");
            uiTooLag = ResourceHelper.FindStringResource("LogLagWarn");

            new Thread(DataShowTask).Start();
        }

        private bool DoInvoke(Action action)
        {
            if (Global.isMainWindowsClosed)
                return false;
            Application.Current.Dispatcher.Invoke(action);
            return true;
        }

        public void ClearContent()
        {
            FlowDocumentScrollViewer.Document.Blocks.Clear();
        }

        /// <summary> 分发显示数据的任务 </summary>
        private void DataShowTask()
        {
            waitQueue.Reset();
            Global.ProgramClosedEvent += (_, _) =>
            {
                waitQueue.Set();
            };
            while (true)
            {
                waitQueue.WaitOne();
                if (Global.isMainWindowsClosed)
                    return;
                var logList = new List<DataShowCommon>();
                lock (DataQueue)//取数据
                {
                    for (int i = 0; i < DataQueue.Count; i++)
                        logList.Add(DataQueue[i]);
                    DataQueue.Clear();
                }
                waitQueue.Reset();

                if (logList.Count == 0)//没数据，切走
                    continue;

                //缓存处理好的数据
                var rawList = new List<DataShowCommon>();
                var uartList = new List<DataShowCommon>();

                DateTime uartSentTime = DateTime.MinValue;

                DateTime uartReceivedTime = DateTime.MinValue;

                for (int i = 0; i < logList.Count; i++)
                {
                    rawList.Add(logList[i]);
                }

                //显示数据
                if (rawList.Count == 0 && uartList.Count == 0)
                    continue;
                //记录一下开始的时间
                var start = DateTime.Now;
                if (!DoInvoke(() =>
                {
                    var count = One.Toolbox.Helpers.ConfigHelper.Instance.AllConfig.SerialportSetting.MaxPacksAutoClear;
                    //条目过多，自动清空
                    if (FlowDocumentScrollViewer.Document.Blocks.Count > count)
                    {
                        FlowDocumentScrollViewer.Document.Blocks.Clear();
                        Paragraph p = new Paragraph(new Run(logAutoClearWarn));
                        FlowDocumentScrollViewer.Document.Blocks.Add(p);
                    }
                    //禁止选中
                    FlowDocumentScrollViewer.IsEnabled = false;
                    for (int i = 0; i < rawList.Count; i++)
                        ShowDataToUI(rawList[i]);
                    //for (int i = 0; i < uartList.Count; i++)
                    //    addUartLog(uartList[i]);
                    if (!LockLog)//如果允许拉到最下面
                        DoInvoke(sv.ScrollToBottom);
                    if (!FlowDocumentScrollViewer.IsMouseOver)
                        FlowDocumentScrollViewer.IsEnabled = true;
                }))
                    return;
                //如果卡顿超过了半秒，则触发自动清空

                var autoClear = One.Toolbox.Helpers.ConfigHelper.Instance.AllConfig.SerialportSetting.LagAutoClear;
                if (autoClear && (DateTime.Now - start).Milliseconds > 250)
                {
                    DoInvoke(() =>
                    {
                        FlowDocumentScrollViewer.Document.Blocks.Clear();
                        Paragraph p = new Paragraph(new Run(uiTooLag));
                        FlowDocumentScrollViewer.Document.Blocks.Add(p);
                    });
                }
                //正常就延时10ms，防止卡住ui线程
                Thread.Sleep(10);
            }
        }

        private void ShowDataToUI(DataShowCommon dataObj)
        {
            Paragraph p = new Paragraph(new Run(""));
            Span text = new Span(new Run(dataObj.TimeToString()));

            text.Foreground = Brushes.DarkSlateGray;
            p.Inlines.Add(text);

            if (!string.IsNullOrEmpty(dataObj.Title))
            {
                text = new Span(new Run(dataObj.Title));
                text.Foreground = dataObj.PrefixColor;
                text.FontWeight = FontWeights.Bold;
                p.Inlines.Add(text);
            }

            if (!string.IsNullOrEmpty(dataObj.Prefix))
            {
                text = new Span(new Run(dataObj.Prefix));
                text.Foreground = dataObj.PrefixColor;
                text.FontWeight = FontWeights.Bold;
                p.Inlines.Add(text);
            }
            //FlowDocumentScrollViewer.Document.Blocks.Add(p);

            if (!string.IsNullOrEmpty(dataObj.Content) )
            {
                //主要显示数据
                //p = new Paragraph(new Run(""));
                text = new Span(new Run(dataObj.Content));

                text.Foreground = dataObj.MessageColor;
                text.FontSize = 15;
                p.Inlines.Add(text);

                //同时显示模式时，才显示小字hex
                /*
                if (dataRaw.hex != null)
                {
                    p = new Paragraph(new Run(dataRaw.hex));
                    p.Foreground = dataRaw.color;
                    p.Margin = new Thickness(0, 0, 0, 8);
                    FlowDocumentScrollViewer.Document.Blocks.Add(p);
                }
                */
            }
            FlowDocumentScrollViewer.Document.Blocks.Add(p);
        }

        /// <summary> 添加一个日志数据到缓冲区 </summary>
        /// <param name="sender"> </param>
        /// <param name="e">      </param>
        public void DataShowAdd(DataShowCommon e)
        {
            lock (DataQueue)
            {
                if (e is DataShowCommon)
                {
                   

                    NLogger.Info($"[{e.CurrentTime}]{(e as DataShowCommon).Prefix}\r\n" + $"{e.Content}\r\n");
                }

                if (DataQueue.Count > 100)
                {
                    DataQueue.Clear();
                    DataQueue.Add(new DataShowCommon
                    {
                        Prefix = packsTooMuch
                    });
                    //延时0.5秒，防止卡住ui线程
                    Thread.Sleep(500);
                }
                else
                    DataQueue.Add(e);
            }
            waitQueue.Set();
        }
    }
}