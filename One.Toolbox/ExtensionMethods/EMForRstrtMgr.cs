using One.Toolbox.Helpers;

namespace One.Toolbox.ExtensionMethods;


public static class EMForRstrtMgr
{
    /// <summary> 复制指定长度的字符到新的字符串 </summary>
    /// <param name="str">    </param>
    /// <param name="start">  </param>
    /// <param name="length"> </param>
    /// <returns> </returns>
    public static void Judge(this Vanara.PInvoke.Win32Error error)
    {
        if (error != Vanara.PInvoke.Win32Error.ERROR_SUCCESS)
        {
            MessageShowHelper.ShowErrorMessage($"Error Code:{error.ToString()}");
        }
    }
}
