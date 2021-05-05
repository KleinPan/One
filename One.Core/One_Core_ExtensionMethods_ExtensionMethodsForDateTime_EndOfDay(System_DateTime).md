#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForDateTime](One_Core_ExtensionMethods_ExtensionMethodsForDateTime.md 'One.Core.ExtensionMethods.ExtensionMethodsForDateTime')
## ExtensionMethodsForDateTime.EndOfDay(DateTime) Method
获取当天最后一刻 "23:59:59:999" The last moment of the day. Use "DateTime2" column type in sql to keep the precision. 
```csharp
public static System.DateTime EndOfDay(this System.DateTime @this);
```
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForDateTime_EndOfDay(System_DateTime)_this'></a>
`this` [System.DateTime](https://docs.microsoft.com/en-us/dotnet/api/System.DateTime 'System.DateTime')  
The @this to act on. 
  
#### Returns
[System.DateTime](https://docs.microsoft.com/en-us/dotnet/api/System.DateTime 'System.DateTime')  
A DateTime of the day with the time set to "23:59:59:999". 
