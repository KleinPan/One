#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForString](One_Core_ExtensionMethods_ExtensionMethodsForString.md 'One.Core.ExtensionMethods.ExtensionMethodsForString')
## ExtensionMethodsForString.IsNullOrEmptyStr(string) Method
验证是否为空字符串。若无需裁切两端空格，建议直接使用 String.IsNullOrEmpty(string) 
```csharp
public static bool IsNullOrEmptyStr(this string str);
```
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForString_IsNullOrEmptyStr(string)_str'></a>
`str` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')  
  
#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')  
### Remarks
不同于String.IsNullOrEmpty(string)，此方法会增加一步Trim操作。如 IsNullOrEmptyStr(" ") 将返回 true。 
