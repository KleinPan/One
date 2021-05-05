#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForString](One_Core_ExtensionMethods_ExtensionMethodsForString.md 'One.Core.ExtensionMethods.ExtensionMethodsForString')
## ExtensionMethodsForString.GetFileSizeFromString(string) Method
将形如 10.1MB 格式对用户友好的文件大小字符串还原成真实的文件大小，单位为字节。 
```csharp
public static long GetFileSizeFromString(this string formatedSize);
```
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_GetFileSizeFromString(string)_formatedSize'></a>
`formatedSize` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
形如 10.1MB 格式的文件大小字符串 
  
#### Returns
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')  
### Remarks
参见： [uoLib.Common.Functions.FormatFileSize(long)](https://docs.microsoft.com/en-us/dotnet/api/uoLib.Common.Functions.FormatFileSize#uoLib_Common_Functions_FormatFileSize_long_ 'uoLib.Common.Functions.FormatFileSize(long)')
