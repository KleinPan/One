using Avalonia;
using Avalonia.Controls;

using System.IO;

namespace One.Toolbox.Helpers
{
    internal class ResourceHelper
    {
        public static string FindStringResource(string resource)
        {
            return (string)FindObjectResource(resource) ?? "?!"; ;
        }

        public static object FindObjectResource(object resourceKey)
        {
            Application.Current.TryFindResource(resourceKey, out object a);
            return a;
        }
    }
}