#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods')
## ExtensionMethodsForString Class
扩展字符串类 
```csharp
public static class ExtensionMethodsForString
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ExtensionMethodsForString  
### Methods

***
[Change16To2(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Change16To2(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Change16To2(string)')

16进制转二进制 

***
[Change2To10(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Change2To10(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Change2To10(string)')

二进制转十进制 

***
[Change2ToBytes(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Change2ToBytes(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Change2ToBytes(string)')

二进制转换byte[]数组 

***
[Change2ToIndex(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Change2ToIndex(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Change2ToIndex(string)')

二进制转化为索引id数据,从右到左 

***
[ClearDecimal0(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ClearDecimal0(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ClearDecimal0(string)')

去除小数位最后为0的 

***
[ClearNoInt(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ClearNoInt(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ClearNoInt(string)')

过滤字符串的非int,重新组合成字符串 

***
[ClearNoInt(string, char)](One_Core_ExtensionMethods_ExtensionMethodsForString_ClearNoInt(string_char).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ClearNoInt(string, char)')

过滤字符串的非int,重新组合成字符串 

***
[ClearPathUnsafe(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ClearPathUnsafe(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ClearPathUnsafe(string)')

删除文件名或路径的特殊字符 

***
[CutStr(string, int)](One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.CutStr(string, int)')

裁切字符串（中文按照两个字符计算，裁切前会先过滤 Html 标签） 

***
[CutStr(string, int, bool)](One_Core_ExtensionMethods_ExtensionMethodsForString_CutStr(string_int_bool).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.CutStr(string, int, bool)')

裁切字符串（中文按照两个字符计算） 

***
[GetFileSizeFromString(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_GetFileSizeFromString(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.GetFileSizeFromString(string)')

将形如 10.1MB 格式对用户友好的文件大小字符串还原成真实的文件大小，单位为字节。 

***
[GetLength(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_GetLength(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.GetLength(string)')

获取字符串长度。与string.Length不同的是，该方法将中文作 2 个字符计算。 

***
[Isbyte(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Isbyte(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Isbyte(string)')

是否是byte true:是 false:否 

***
[IsDateTime(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsDateTime(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsDateTime(string)')

是否是DateTime true:是 false:否 

***
[IsDecimal(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsDecimal(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsDecimal(string)')

是否是Decimal true:是 false:否 

***
[IsDouble(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsDouble(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsDouble(string)')

是否是Double true:是 false:否 

***
[IsInt(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsInt(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsInt(string)')

是否是Int true:是 false:否 

***
[IsInt16(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsInt16(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsInt16(string)')

是否是Int true:是 false:否 

***
[IsIntArr(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsIntArr(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsIntArr(string)')

是否可以转换成int[],true:是,false:否 

***
[IsIntArr(string, char)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsIntArr(string_char).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsIntArr(string, char)')

是否可以转换成int[],true:是,false:否 

***
[IsLong(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsLong(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsLong(string)')

是否是Long true:是 false:否 

***
[IsNullOrEmptyStr(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsNullOrEmptyStr(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsNullOrEmptyStr(string)')

验证是否为空字符串。若无需裁切两端空格，建议直接使用 String.IsNullOrEmpty(string) 

***
[IsNumeric(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsNumeric(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsNumeric(string)')

验证字符串是否由正负号（+-）、数字、小数点构成，并且最多只有一个小数点 

***
[IsNumericOnly(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsNumericOnly(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsNumericOnly(string)')

验证字符串是否仅由[0-9]构成 

***
[IsNumericOrLetters(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_IsNumericOrLetters(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.IsNumericOrLetters(string)')

验证字符串是否由字母和数字构成 

***
[Left(string, int, string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Left(string_int_string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Left(string, int, string)')

载取左字符 

***
[LengthReal(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_LengthReal(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.LengthReal(string)')

字符串真实长度 如:一个汉字为两个字节 

***
[Tobyte(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_Tobyte(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Tobyte(string)')

转byte,失败返回0 

***
[Tobyte(string, byte)](One_Core_ExtensionMethods_ExtensionMethodsForString_Tobyte(string_byte).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.Tobyte(string, byte)')

转byte,失败返回pReturn 

***
[ToByte(string, Encoding)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToByte(string_System_Text_Encoding).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToByte(string, System.Text.Encoding)')

转byte[]

***
[ToDateTime(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDateTime(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDateTime(string)')

转DateTime,失败返回当前时间 

***
[ToDateTime(string, string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDateTime(string_string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDateTime(string, string)')

转DateTime,失败返回空 

***
[ToDateTime(string, string, string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDateTime(string_string_string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDateTime(string, string, string)')

转DateTime,失败返回pReturn 

***
[ToDateTime(string, DateTime)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDateTime(string_System_DateTime).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDateTime(string, System.DateTime)')

转DateTime,失败返回pReturn 

***
[ToDecimal(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDecimal(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDecimal(string)')

转Decimal,失败返回0 

***
[ToDecimal(string, decimal)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDecimal(string_decimal).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDecimal(string, decimal)')

转Decimal,失败返回pReturn 

***
[ToDouble(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDouble(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDouble(string)')

转Int,失败返回0 

***
[ToDouble(string, double)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToDouble(string_double).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToDouble(string, double)')

转Double,失败返回pReturn 

***
[ToInt(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToInt(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToInt(string)')

转Int,失败返回0  

***
[ToInt(string, int)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToInt(string_int).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToInt(string, int)')

转Int,失败返回pReturn  

***
[ToInt16(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToInt16(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToInt16(string)')

转Int,失败返回0 

***
[ToInt16(string, short)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToInt16(string_short).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToInt16(string, short)')

转Int,失败返回pReturn 

***
[ToIntArr(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToIntArr(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToIntArr(string)')

转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错 

***
[ToIntArr(string, char[])](One_Core_ExtensionMethods_ExtensionMethodsForString_ToIntArr(string_char__).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToIntArr(string, char[])')

转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错 

***
[ToLong(string)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToLong(string).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToLong(string)')

转Long,失败返回0 

***
[ToLong(string, long)](One_Core_ExtensionMethods_ExtensionMethodsForString_ToLong(string_long).md 'One.Core.ExtensionMethods.ExtensionMethodsForString.ToLong(string, long)')

转Long,失败返回pReturn 
