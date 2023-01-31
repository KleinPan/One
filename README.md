<div align="center">

# One (一个就够了)

</div>

[![One.Core](https://img.shields.io/nuget/v/One.Core?label=One.Core)](https://www.nuget.org/packages/One.Core/)
&nbsp; [![Nuget](https://img.shields.io/nuget/v/One.Control?label=One.Control)](https://www.nuget.org/packages/One.Control/)
&nbsp; [![Nuget](https://img.shields.io/nuget/v/One.AutoUpdater?label=One.AutoUpdater)](https://www.nuget.org/packages/One.AutoUpdater/)
&nbsp; ![GitHub issues](https://img.shields.io/github/issues/KleinPan/One)
&nbsp; [![博客地址](https://img.shields.io/badge/cnblogs-Link-brightgreen")](https://www.cnblogs.com/KevinBran/)


<!--
  ## Nuget Links
  
  | [One.Core](https://www.nuget.org/packages/One.Core/)  | [One.Control](https://www.nuget.org/packages/One.Control/) 
  | ------------- | ------------- 
  
-->

![Alt](https://repobeats.axiom.co/api/embed/4fb7dc32557eadd8782eafb3e3f4564a73996dd1.svg "Repobeats analytics image")

## 功能介绍
### 1. One.Core
    -BaseClass
        BindableObject
    -ExtensionMethods
        Array
          //复制指定长度的字符到新的字符串
          public static string[] Copy(this Array str, int start, int length)
          
          //Array添加单项
          public static T[] Add<T>(this T[] array, T item)
        
          //Array添加多项
          public static T[] AddRange<T>(this T[] sourceArray, T[] addArray)
        Byte
          //转换当前byte[]为对应的string
          public static string ToString(this byte[] args, System.Text.Encoding encoding)
        
        Collection
          //添加T类型的序列到当前序列末尾（带通知）
          public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
          
          //添加T类型的序列到当前序列末尾
          public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        DateTime
          //获取当天起始时间 00:00:00:000
          public static DateTime StartOfDay(this DateTime @this)
          
          //获取当天最后一刻 "23:59:59:999"
          public static DateTime EndOfDay(this DateTime @this)
        Enum
          //获取枚举描述属性
          public static string GetCustomAttributeDescription(this Enum value)
          
          //解析枚举
          public static T TryParse<T>(this string value) where T : struct
        IEnumerate
          //返回指定筛选条件下序列非重复元素的首项
          public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
          
          //获取指定筛选条件下对象的Index
          public static int FindIndex<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
          
          //Enumerates for each in this collection.
          public static IEnumerable<T> ForEach<T>(this IEnumerable<T> @this, Action<T> action)
          
          //统计分组数量
          public static int GroupCount<TTempClass, Telement>(this IEnumerable<IGrouping<TTempClass, Telement>> source)
          
          //Adds each item in the <see cref="IEnumerator"/> into a <see cref="List{T}"/> and return the new <see cref="List{T}"
          public static List<T> ToList<T>(this IEnumerator enumerator)
        ExtensionMethodsForString
        //数据转换
        
        //载取左字符
        
        //删除文件名或路径的特殊字符
        
        //进制转换
        
        //...
### 2. One.Control
#### 合并多Style
##### 引入命名空间Nuget
 `xmlns:ex="clr-namespace:One.Control.ExtentionMethods;assembly=One.Control"`
 #####   应用样式
目前有两种方式。一种是通过MarkupExtension扩展，这种有点儿缺陷，在设计时不能直接实时显示效果，需要把 Style 剪切再粘贴才会显示真正的效果。一种是通过附加属性，这个是实时显示效果的。 以Button 为例：
```
<Button Style="{ex:MultiStyle btn btn-default btn-lg}" Content="Large button"/>

<Button ex:Apply.MultiStyle="btn btn-primary btn-lg" Content="Large button"/>
```
### 3. One.AutoUpdater
#### 使用方法
```
System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential()
  {
    UserName = "UpdateUser",
    Password="123456",
  };

One.AutoUpdater.AutoUpdater.Start("ftp://114.215.94.141//UpdateDirectories//Version.json", networkCredential);
 ```

#### 格式说明
推荐使用Json格式
```
{
   "CurrentVersion":"1.1.0.0",
   "DownloadURL":"ftp://114.215.94.141/Update.zip",
   "ChangelogURL":"https://github.com/ravibpatel/AutoUpdater.NET/releases",
   "Mandatory":{
      "Value":false,
      "MinimumVersion": "1.1.0.0"
   },
   "CheckSum":{
      "Value":"C5A5DF71B99285E8C98B1AB3BD49025FA0B800A3",
      "HashingAlgorithm":"SHA1"
   }
}
