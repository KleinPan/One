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

# 功能介绍
## 1. One.Core
## 2. One.Control
### 合并多Style
#### 引入命名空间Nuget
 `xmlns:ex="clr-namespace:One.Control.ExtentionMethods;assembly=One.Control"`
 ####   应用样式
目前有两种方式。一种是通过MarkupExtension扩展，这种有点儿缺陷，在设计时不能直接实时显示效果，需要把 Style 剪切再粘贴才会显示真正的效果。一种是通过附加属性，这个是实时显示效果的。 以Button 为例：
```
<Button Style="{ex:MultiStyle btn btn-default btn-lg}" Content="Large button"/>

<Button ex:Apply.MultiStyle="btn btn-primary btn-lg" Content="Large button"/>
```
## 3. One.AutoUpdater
### 使用方法
```
System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential()
  {
    UserName = "UpdateUser",
    Password="123456",
  };

One.AutoUpdater.AutoUpdater.Start("ftp://114.215.94.141//UpdateDirectories//Version.json", networkCredential);
 ```

### 格式说明
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
```

## 4. One.Toolbox
![image](https://github.com/KleinPan/One/blob/master/docs/Dashboard.jpg?raw=true)

### 字符处理工具集
![image](https://github.com/KleinPan/One/blob/427f8f332a826520d4f9f829a6515de9dadbf34f/docs/Serialport.jpg)

### 显示编号
![image](https://github.com/KleinPan/One.Toolbox/assets/34428802/23e2cc12-f89e-47bf-af0a-1c983900c337)

### 串口工具
待完善

### 网络工具
待完善



