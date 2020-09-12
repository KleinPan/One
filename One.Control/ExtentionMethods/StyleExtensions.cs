using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace One.Control.ExtentionMethods
{
    /// <summary>
    /// 一些扩展方法。
    /// </summary>
    public static class StyleExtensions
    {
        /// <summary>
        /// 合并指定的 <see cref="System.Windows.Style"/>。
        /// </summary>
        /// <param name="style1"></param>
        /// <param name="style2"></param>
        public static void Merge(this Style style1, Style style2)
        {
            if (style1 == null)
            {
                throw new ArgumentNullException("style1");
            }
            if (style2 == null)
            {
                throw new ArgumentNullException("style2");
            }

            if (style1.TargetType.IsAssignableFrom(style2.TargetType))
            {
                style1.TargetType = style2.TargetType;
            }

            if (style2.BasedOn != null)
            {
                Merge(style1, style2.BasedOn);
            }

            foreach (SetterBase currentSetter in style2.Setters)
            {
                style1.Setters.Add(currentSetter);
            }

            foreach (TriggerBase currentTrigger in style2.Triggers)
            {
                style1.Triggers.Add(currentTrigger);
            }

            // This code is only needed when using DynamicResources.
            foreach (object key in style2.Resources.Keys)
            {
                style1.Resources[key] = style2.Resources[key];
            }
        }

        /// <summary>
        /// 集合是否有值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasValue<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static ResourceDictionary FindResourceDictionary(this Style style, object resourceKey)
        {
            if (style.Resources != null && style.Resources.Contains(resourceKey))
            {
                return style.Resources;
            }
            if (style.BasedOn != null)
            {
                return style.BasedOn.FindResourceDictionary(resourceKey);
            }
            return null;
        }

        public static void Merge(this ResourceDictionary resourceDictionary, ResourceDictionary mergee)
        {
            if (resourceDictionary != null && mergee != null && mergee.Count > 0)
            {
                foreach (var key in mergee.Keys)
                {
                    if (!resourceDictionary.Contains(key))
                    {
                        resourceDictionary.Add(key, mergee[key]);
                    }
                }
            }
        }
    }
}
