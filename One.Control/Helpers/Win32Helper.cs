using System.Runtime.InteropServices;

namespace One.Control.Helpers
{
    public static class Win32Helper
    {
        public struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// 该函数检取光标的位置，以屏幕坐标表示。
        /// <para> 光标的位置通常以屏幕坐标的形式给出，它并不受包含该光标的窗口的映射模式的影响。 </para>
        /// </summary>
        /// <param name="point"> </param>
        /// <returns> </returns>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT point);
    }
}