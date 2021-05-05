#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForString](One_Core_ExtensionMethods_ExtensionMethodsForString.md 'One.Core.ExtensionMethods.ExtensionMethodsForString')
## ExtensionMethodsForString.CutStr(string, int) Method
裁切字符串（中文按照两个字符计算，裁切前会先过滤 Html 标签） 
```csharp
public static string CutStr(this string str, int len);
```
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int)_str'></a>
`str` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
旧字符串 
  
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int)_len'></a>
`len` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')  
新字符串长度 
  
#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
### Remarks


注意： <ol>
  <li> 若字符串被截断则会在末尾追加“...”，反之则直接返回原始字符串。 </li>
  <li> 中文按照两个字符计算。若指定长度位置恰好只获取半个中文字符，则会将其补全，如下面的例子： <br /><code><![CDATA[
            string str = "感谢使用uoLib模块。";
            string A = CutStr(str,4);   // A = "感谢..."
            string B = CutStr(str,5);   // B = "感谢使..."
            ]]></code></li>
</ol>
