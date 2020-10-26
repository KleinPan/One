


<div align="center">

# One (一个就够了)

</div>

* One.Core &nbsp; [![Nuget](https://img.shields.io/nuget/v/One.Core)](https://www.nuget.org/packages/One.Core/)
* One.Control &nbsp; [![Nuget](https://img.shields.io/nuget/v/One.Control)](https://www.nuget.org/packages/One.Control/)
* One.AutoUpdater &nbsp; [![Nuget](https://img.shields.io/nuget/v/One.AutoUpdater )](https://www.nuget.org/packages/One.AutoUpdater /)

* ![GitHub issues](https://img.shields.io/github/issues/KleinPan/One)

* [![博客地址](https://img.shields.io/badge/cnblogs-Link-brightgreen")](https://www.cnblogs.com/KevinBran/)


## Tools

We use these awesome tools to build and develop One.

<div>
  <a href="https://www.jetbrains.com/resharper/">
    <img alt="R#" width="128" heigth="128" vspace="20" hspace="20" src="./docs/icon_ReSharper.png">
  </a>
</div>

<!--
  ## Nuget Links
  
  | [One.Core](https://www.nuget.org/packages/One.Core/)  | [One.Control](https://www.nuget.org/packages/One.Control/) 
  | ------------- | ------------- 
  
-->
## 功能介绍
### 合并多Style
#### 使用
##### 引入命名空间Nuget
 `xmlns:ex="clr-namespace:One.Control.ExtentionMethods;assembly=One.Control"`
 #####   应用样式
目前有两种方式。一种是通过MarkupExtension扩展，这种有点儿缺陷，在设计时不能直接实时显示效果，需要把 Style 剪切再粘贴才会显示真正的效果。一种是通过附加属性，这个是实时显示效果的。 以Button 为例：
```
<Button Style="{ex:MultiStyle btn btn-default btn-lg}" Content="Large button"/>

<Button ex:Apply.MultiStyle="btn btn-primary btn-lg" Content="Large button"/>
```

