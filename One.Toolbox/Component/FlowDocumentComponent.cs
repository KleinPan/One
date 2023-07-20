using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.Models;
using One.Toolbox.Tools;

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace One.Toolbox.Component
{
    /// <summary> 来什么显示什么，不做处理 </summary>
    internal class FlowDocumentComponent
    {
        public static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();
        private FlowDocumentScrollViewer FlowDocumentScrollViewer { get; set; }

        /// <summary> 数据缓冲 </summary>
        private List<DataShow> DataQueue = new List<DataShow>();

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
                var logList = new List<DataShow>();
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
                var rawList = new List<DataRaw>();
                var uartList = new List<DataUart>();

                DateTime uartSentTime = DateTime.MinValue;
                //var uartSentList = new List<string>();
                DateTime uartReceivedTime = DateTime.MinValue;
                //var uartReceivedList = new List<byte>();

                for (int i = 0; i < logList.Count; i++)
                {
                    if (logList[i] as DataShowRaw != null)
                        rawList.Add(new DataRaw(logList[i] as DataShowRaw));
                    else
                    {
                        //串口数据收发分一下，后续可以合并数据
                        var d = logList[i] as DataShowPara;

                        DataUart sentData = null;
                        DataUart receivedData = null;

                        if (d.send)
                        {
                            uartSentTime = d.time;
                            sentData = new DataUart(d.data, d.time, true);
                        }
                        else
                        {
                            uartReceivedTime = d.time;
                            receivedData = new DataUart(d.data, d.time, false);
                        }

                        //包的时间顺序要对
                        if (sentData == null && receivedData != null)
                            uartList.Add(receivedData);
                        else if (sentData != null && receivedData == null)
                            uartList.Add(sentData);
                        else if (sentData != null && receivedData != null)
                        {
                            if (uartSentTime < uartReceivedTime)
                            {
                                uartList.Add(sentData);
                                uartList.Add(receivedData);
                            }
                            else
                            {
                                uartList.Add(receivedData);
                                uartList.Add(sentData);
                            }
                        }
                    }
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
                        DataShowRaw(rawList[i]);
                    for (int i = 0; i < uartList.Count; i++)
                        addUartLog(uartList[i]);
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

        private void DataShowRaw(DataRaw dataRaw)
        {
            Paragraph p = new Paragraph(new Run(""));
            Span text = new Span(new Run(dataRaw.time));
            text.Foreground = Brushes.DarkSlateGray;
            p.Inlines.Add(text);
            text = new Span(new Run(dataRaw.title));
            text.Foreground = Brushes.Black;
            text.FontWeight = FontWeights.Bold;
            p.Inlines.Add(text);
            FlowDocumentScrollViewer.Document.Blocks.Add(p);

            if (dataRaw.data != null)//有数据时才显示信息
            {
                //主要显示数据
                p = new Paragraph(new Run(""));
                text = new Span(new Run(dataRaw.data));
                text.Foreground = dataRaw.color;
                text.FontSize = 15;
                p.Inlines.Add(text);
                FlowDocumentScrollViewer.Document.Blocks.Add(p);

                //同时显示模式时，才显示小字hex
                if (dataRaw.hex != null)
                {
                    p = new Paragraph(new Run(dataRaw.hex));
                    p.Foreground = dataRaw.color;
                    p.Margin = new Thickness(0, 0, 0, 8);
                    FlowDocumentScrollViewer.Document.Blocks.Add(p);
                }
            }
        }

        /// <summary> 添加串口日志数据 </summary>
        /// <param name="data"> 数据 </param>
        /// <param name="send"> true为发送，false为接收 </param>
        private void addUartLog(DataUart d)
        {
            var temp = One.Toolbox.Helpers.ConfigHelper.Instance.AllConfig.SerialportSetting.Timeout;
            if (temp >= 0)
            {
                Paragraph p = new Paragraph(new Run(""));

                Span text = new Span(new Run(d.time));
                text.Foreground = Brushes.DarkSlateGray;
                p.Inlines.Add(text);

                text = new Span(new Run(d.title));
                text.Foreground = Brushes.Black;
                text.FontWeight = FontWeights.Bold;
                p.Inlines.Add(text);

                //主要显示数据
                text = new Span(new Run(d.data));
                text.Foreground = d.color;
                text.FontSize = 15;
                p.Inlines.Add(text);

                //同时显示模式时，才显示小字hex
                if (d.hex != null)
                    p.Margin = new Thickness(0, 0, 0, 8);
                FlowDocumentScrollViewer.Document.Blocks.Add(p);

                //同时显示模式时，才显示小字hex
                if (d.hex != null)
                {
                    p = new Paragraph(new Run(d.hex));
                    p.Foreground = d.hexColor;
                    p.Margin = new Thickness(0, 0, 0, 8);
                    FlowDocumentScrollViewer.Document.Blocks.Add(p);
                }
            }
            else//不分包
            {
                if (FlowDocumentScrollViewer.Document.Blocks.LastBlock == null ||
                   FlowDocumentScrollViewer.Document.Blocks.LastBlock.GetType() != typeof(Paragraph))
                    FlowDocumentScrollViewer.Document.Blocks.Add(new Paragraph(new Run("")));

                //待显示的数据
                string s;
                if (Global.setting.showHexFormat == 2 && d.hex != null)
                    s = d.hex;
                else
                    s = d.data;
                Span text = new Span(new Run(s));
                text.FontSize = 15;
                text.Foreground = d.color;
                (FlowDocumentScrollViewer.Document.Blocks.LastBlock as Paragraph).Inlines.Add(text);
            }

            if (!LockLog)//如果允许拉到最下面
                sv.ScrollToBottom();
            FlowDocumentScrollViewer.IsSelectionEnabled = true;
        }

        /// <summary> 添加一个日志数据到缓冲区 </summary>
        /// <param name="sender"> </param>
        /// <param name="e">      </param>
        public void DataShowAdd(DataShow e)
        {
            lock (DataQueue)
            {
                if (e is DataShowRaw)
                {
                    //Logger.AddUartLogInfo($"[{e.time}]{(e as Tools.DataShowRaw).title}\r\n" +
                    //    $"{Global.GetEncoding().GetString(e.data)}\r\n" +
                    //    $"HEX:{Tools.Global.Byte2Hex(e.data, " ")}");

                    NLogger.Info($"[{e.time}]{(e as DataShowRaw).title}\r\n" + $"e.data\r\n");
                }

                if (DataQueue.Count > 100)
                {
                    DataQueue.Clear();
                    DataQueue.Add(new DataShowRaw
                    {
                        title = packsTooMuch
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