#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForString](One_Core_ExtensionMethods_ExtensionMethodsForString.md 'One.Core.ExtensionMethods.ExtensionMethodsForString')
## ExtensionMethodsForString.CutStr(string, int, bool) Method
裁切字符串（中文按照两个字符计算） 
```csharp
public static string CutStr(this string str, int len, bool HtmlEnable);
```
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int_bool)_str'></a>
`str` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
旧字符串 
  
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int_bool)_len'></a>
`len` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')  
新字符串长度 
  
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int_bool)_HtmlEnable'></a>
`HtmlEnable` [System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')  
为 false 时过滤 Html 标签后再进行裁切，反之则保留 Html 标签。 
  
#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
### Remarks


注意： <ol>
  <li> 若字符串被截断则会在末尾追加“...”，反之则直接返回原始字符串。 </li>
  <li> 参数 <paramref name="HtmlEnable" /> 为 false 时会先调用 <see cref="!:uoLib.Common.Functions.HtmlFilter" /> 过滤掉 Html 标签再进行裁切。 </li>
  <li> 中文按照两个字符计算。若指定长度位置恰好只获取半个中文字符，则会将其补全，如下面的例子： <br /><code><![CDATA[
            string str = "感谢使用uoLib。";
            string A = CutStr(str,4);   // A = "感谢..."
            string B = CutStr(str,5);   // B = "感谢使..."
            ]]></code></li>
</ol>
