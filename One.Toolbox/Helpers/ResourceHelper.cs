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
            return Application.Current.TryFindResource(resourceKey);
        }

        /// <summary> 读取软件资源文件内容 </summary>
        /// <param name="path"> 路径 </param>
        /// <returns> 内容字节数组 </returns>
        public static byte[] GetAssetsFileContent(string path)
        {
            Uri uri = new Uri(path, UriKind.Relative);
            var source = System.Windows.Application.GetResourceStream(uri).Stream;
            byte[] f = new byte[source.Length];
            source.Read(f, 0, (int)source.Length);
            return f;
        }

        /// <summary> 取出文件 </summary>
        /// <param name="insidePath"> 软件内部的路径 </param>
        /// <param name="outPath">    需要释放到的路径 </param>
        /// <param name="d">          是否覆盖 </param>
        public static void CreateFile(string insidePath, string outPath, bool d = true)
        {
            if (!File.Exists(outPath) || d)
                File.WriteAllBytes(outPath, GetAssetsFileContent(insidePath));
        }

        public static ResourceDictionary Dic = new ResourceDictionary { Source = new Uri(@"Resources/Themes/Basic/Geometries.xaml", UriKind.Relative) };
    }
}