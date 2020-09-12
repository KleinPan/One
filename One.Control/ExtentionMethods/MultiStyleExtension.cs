using System;
using System.Windows;
using System.Windows.Markup;

namespace One.Control.ExtentionMethods
{
    /// <summary> 实现一个标记扩展，该标记扩展支持根据 XAML 制作的多个静态（XAML 加载时） <see cref="System.Windows.Style"/> 资源引用。 </summary>
    [MarkupExtensionReturnType(typeof(Style))]
    public class MultiStyleExtension : MarkupExtension
    {
        private string[] _internalResourceKeys = new string[0];
        private string _resourceKeys;

        [ConstructorArgument("resourceKeys")]
        public string ResourceKeys
        {
            get
            {
                return _resourceKeys;
            }

            set
            {
                _resourceKeys = value;

                if (_resourceKeys.HasValue())
                {
                    _internalResourceKeys = _resourceKeys.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    _internalResourceKeys = new string[0];
                }
            }
        }

        /// <summary> 构造方法。 </summary>
        public MultiStyleExtension()
        {
        }

        /// <summary> 构造方法。 </summary>
        /// <param name="resourceKeys"> 多个 <see cref="System.Windows.Style"/> 资源字典多个 Key </param>
        public MultiStyleExtension(string resourceKeys)
        {
            ResourceKeys = resourceKeys;
        }

        /// <summary> 返回一个应在此扩展应用的属性上设置的对象。对于 <see cref="WpfMultiStyle.MultiStyleExtension"/>，这是在资源字典中查找的多个 <see cref="System.Windows.Style"/> 对象，并合并这些对象，其中要查找的对象由 <see cref="System.Windows.StaticResourceExtension.ResourceKey"/> 标识。 </summary>
        /// <param name="serviceProvider"> 可以为标记扩展提供服务的对象。 </param>
        /// <returns> 要在计算标记扩展提供的值的属性上设置的对象值。 </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Style resultStyle = new Style();

            IProvideValueTarget service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (service.TargetObject is FrameworkElement fe)
            {
                if (fe != null)
                {
                    foreach (string resourceKey in _internalResourceKeys)
                    {
                        Style currentStyle = fe.TryFindResource(resourceKey) as Style;

                        // 忽略无效的 Style
                        if (currentStyle != null)
                        {
                            resultStyle.Merge(currentStyle);
                        }
                    }
                }
            }
            //可以合并Basedon上的style
            else if (service.TargetObject is Style style)
            {
                foreach (string resourceKey in _internalResourceKeys)
                {
                    Style currentStyle = Application.Current.TryFindResource(resourceKey) as Style;

                    // 忽略无效的 Style
                    if (currentStyle != null)
                    {
                        resultStyle.Merge(currentStyle);
                    }
                }
            }

            return resultStyle;
        }
    }
}