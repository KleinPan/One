#### [One.Core](index.md 'index')
### [One.Core.Helper.HttpHelper](One_Core_Helper_HttpHelper.md 'One.Core.Helper.HttpHelper').[HTTPClientHelper](One_Core_Helper_HttpHelper_HTTPClientHelper.md 'One.Core.Helper.HttpHelper.HTTPClientHelper')
## HTTPClientHelper.HttpPostRequestAsync(string, Dictionary&lt;string,string&gt;, string) Method
异步POST请求  
```csharp
public static string HttpPostRequestAsync(string Url, System.Collections.Generic.Dictionary<string,string> paramArray, string ContentType="application/x-www-form-urlencoded");
```
#### Parameters
<a name='One_Core_Helper_HttpHelper_HTTPClientHelper_HttpPostRequestAsync(string_System_Collections_Generic_Dictionary_string_string__string)_Url'></a>
`Url` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
234
  
<a name='One_Core_Helper_HttpHelper_HTTPClientHelper_HttpPostRequestAsync(string_System_Collections_Generic_Dictionary_string_string__string)_paramArray'></a>
`paramArray` [System.Collections.Generic.Dictionary&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2 'System.Collections.Generic.Dictionary`2')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2 'System.Collections.Generic.Dictionary`2')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2 'System.Collections.Generic.Dictionary`2')  
324
  
<a name='One_Core_Helper_HttpHelper_HTTPClientHelper_HttpPostRequestAsync(string_System_Collections_Generic_Dictionary_string_string__string)_ContentType'></a>
`ContentType` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  


POST请求的两种编码格式:

"application/x-www-urlencoded"是浏览器默认的编码格式,用于键值对参数,参数之间用&间隔；

"multipart/form-data"常用于文件等二进制，也可用于键值对参数，最后连接成一串字符传输(参考Java OK HTTP)。



除了这两个编码格式，还有"application/json"也经常使用。
  
#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
