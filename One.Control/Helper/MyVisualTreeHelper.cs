using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace One.Control.Helper
{
    /// <summary>
    ///关于WPF界面交互的帮助类
    /// </summary>
    public class MyVisualTreeHelper
    {
        /// <summary>查找可视化树的父元素,包括本级元素</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T)
                {
                    return obj as T;
                }

                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }

        /// <summary>查找可视化树的父元素,不包括本级元素</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualParent2<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                obj = VisualTreeHelper.GetParent(obj);

                if (obj is T)
                {
                    return obj as T;
                }
            }

            return null;
        }
    }

  
}
