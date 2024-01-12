using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;

using Timer = System.Windows.Forms.Timer;

namespace One.Toolbox.Helpers;

internal partial class HideWindowHelper
{
    private readonly Window _window;
    private readonly Timer _timer;
    private readonly List<HideCore> _hideLogicList = new List<HideCore>();

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPos(out Point pt);

    private struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private bool _isHide;
    private bool _isStarted;
    private HideCore _lastHiderOn;
    private bool _isInAnimation;

    private HideWindowHelper(Window window)
    {
        _window = window;

        _timer = new Timer { Interval = 300 };
        _timer.Tick += _timer_Tick;
    }

    public HideWindowHelper AddHider<THideCore>() where THideCore : HideCore, new()
    {
        if (_isStarted) throw new Exception("调用了Start方法后无法在添加隐藏逻辑");
        var logic = new THideCore();
        logic.Init(_window, AnimationReport);
        _hideLogicList.Add(logic);
        return this;
    }

    private void _timer_Tick(object sender, EventArgs e)
    {
        if (_isInAnimation) return;
        if (_window.IsActive) return;
        GetCursorPos(out var point); //获取鼠标相对桌面的位置
        var isMouseEnter = point.X >= _window.Left
                           && point.X <= _window.Left + _window.Width + 30
                           && point.Y >= _window.Top
                           && point.Y <= _window.Top
                           + _window.Height + 30;
        //鼠标在里面
        if (isMouseEnter)
        {
            //没有隐藏，直接返回
            if (!_isHide) return;

            //理论上不会出现为null的情况
            if (_lastHiderOn != null)
            {
                _lastHiderOn.Show();
                _isHide = false;
                _window.ShowInTaskbar = true;
                return;
            }
        }

        foreach (var core in _hideLogicList)
        {
            //鼠标在里面并且没有隐藏
            if (isMouseEnter && !_isHide) return;

            //鼠标在里面并且当期是隐藏状态且当前处理器成功显示了窗体
            if (isMouseEnter && _isHide && core.Show())
            {
                _isHide = false;
                _window.ShowInTaskbar = true;
                return;
            }

            //鼠标在外面并且没有隐藏，那么调用当前处理器尝试隐藏窗体
            if (!isMouseEnter && !_isHide && core.Hide())
            {
                _lastHiderOn = core;
                _isHide = true;
                _window.ShowInTaskbar = false;
                return;
            }
        }
    }

    private void AnimationReport(bool isInAnimation)
    {
        _isInAnimation = isInAnimation;
    }

    /// <summary>
    /// 启动隐藏
    /// </summary>
    /// <returns></returns>
    public HideWindowHelper Start()
    {
        _isStarted = true;
        _timer.Start();
        return this;
    }

    public void Stop()
    {
        _timer.Stop();
        _isStarted = false;
    }

    public static HideWindowHelper CreateFor(Window window)
    {
        return new HideWindowHelper(window);
    }

    /// <summary>
    /// 用代码触发显示，适用于双击任务栏图标
    /// </summary>
    public void TryShow()
    {
        if (_lastHiderOn == null) return;
        _lastHiderOn.Show();
        _isHide = false;
        _window.Activate();
    }
}

#region 隐藏逻辑基类

public abstract class HideCore
{
    private Window _window;
    private Action<bool> _animationStateReport;

    internal void Init(Window window, Action<bool> animationStateReport)
    {
        _window = window;
        _animationStateReport = animationStateReport;
    }

    public abstract bool Show();

    public abstract bool Hide();

    protected Window WindowInstance => _window;

    protected void StartAnimation(DependencyProperty property, double from, double to)
    {
        _animationStateReport(true);
        var doubleAnimation = new DoubleAnimation(from, to, TimeSpan.FromSeconds(0.3));
        doubleAnimation.EasingFunction = new QuadraticEase();//允许你指定动画的缓动效果，可以使窗口的位置变化更加平滑。

        doubleAnimation.Completed += delegate
        {
            _window.BeginAnimation(property, null);
            _animationStateReport(false);
        };
        _window.BeginAnimation(property, doubleAnimation);
    }
}

#endregion 隐藏逻辑基类

#region 向上隐藏

internal class HideOnTop : HideCore
{
    //RenderTransform 的帧动画在某些情况下可能会比直接控制窗口的动画效果更丝滑。这是因为 RenderTransform 的动画是在 GPU 上执行的，而直接控制窗口的动画是在 CPU 上执行的。
    public override bool Show()
    {
        if (WindowInstance.Top > 0) return false;
        StartAnimation(Window.TopProperty, WindowInstance.Top, 0);
        return true;
    }

    public override bool Hide()
    {
        if (WindowInstance.Top > 2) return false;

        StartAnimation(Window.TopProperty, WindowInstance.Top, 0 - WindowInstance.Top - WindowInstance.Height + 2);

        return true;
    }
}

#endregion 向上隐藏

#region 向左隐藏

internal class HideOnLeft : HideCore
{
    public override bool Show()
    {
        if (WindowInstance.Left > 0) return false;
        StartAnimation(Window.LeftProperty, WindowInstance.Left, 0);
        return true;
    }

    public override bool Hide()
    {
        if (WindowInstance.Left > 2) return false;
        StartAnimation(Window.LeftProperty, WindowInstance.Left, 0 - WindowInstance.Width + 2);
        return true;
    }
}

#endregion 向左隐藏

#region 向右隐藏

internal class HideOnRight : HideCore
{
    private readonly int _screenWidth;

    public HideOnRight()
    {
        foreach (var screen in Screen.AllScreens)
        {
            _screenWidth += screen.Bounds.Width;
        }
    }

    public override bool Show()
    {
        if (_screenWidth - WindowInstance.Left - WindowInstance.Width > 0) return false;
        StartAnimation(Window.LeftProperty, WindowInstance.Left, _screenWidth - WindowInstance.Width);
        return true;
    }

    public override bool Hide()
    {
        if (_screenWidth - WindowInstance.Left - WindowInstance.Width > 2) return false;
        StartAnimation(Window.LeftProperty, WindowInstance.Left, _screenWidth - 2);
        return true;
    }
}

#endregion 向右隐藏