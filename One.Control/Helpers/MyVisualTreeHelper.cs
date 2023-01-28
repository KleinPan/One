using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace One.Control.Helpers
{
    /// <summary>
    ///关于WPF界面交互的帮助类
    /// </summary>
    public class MyVisualTreeHelper
    {
        /// <summary> 查找可视化树的父元素,包括本级元素 </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <returns> </returns>
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

        /// <summary> 查找可视化树的父元素,不包括本级元素 </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <returns> </returns>
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

        /// <summary> 查找可视化树的子元素 </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}