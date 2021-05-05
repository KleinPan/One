#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForString](One_Core_ExtensionMethods_ExtensionMethodsForString.md 'One.Core.ExtensionMethods.ExtensionMethodsForString')
## ExtensionMethodsForString.GetLength(string) Method
获取字符串长度。与string.Length不同的是，该方法将中文作 2 个字符计算。 
```csharp
public static int GetLength(this string str);
```
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_GetLength(string)_str'></a>
`str` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
目标字符串 
  
#### Returns
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')  
