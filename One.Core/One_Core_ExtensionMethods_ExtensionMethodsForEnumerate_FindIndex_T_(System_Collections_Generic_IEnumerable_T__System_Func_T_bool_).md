#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForEnumerate](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate.md 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate')
## ExtensionMethodsForEnumerate.FindIndex&lt;T&gt;(IEnumerable&lt;T&gt;, Func&lt;T,bool&gt;) Method
获取指定筛选条件下对象的Index  
```csharp
public static int FindIndex<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,bool> predicate);
```
#### Type parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_)_T'></a>
`T`  
  
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_)_collection'></a>
`collection` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[T](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_).md#One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_)_T 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate.FindIndex&lt;T&gt;(System.Collections.Generic.IEnumerable&lt;T&gt;, System.Func&lt;T,bool&gt;).T')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')  
  
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_)_predicate'></a>
`predicate` [System.Func&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Func-2 'System.Func`2')[T](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_).md#One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_FindIndex_T_(System_Collections_Generic_IEnumerable_T__System_Func_T_bool_)_T 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate.FindIndex&lt;T&gt;(System.Collections.Generic.IEnumerable&lt;T&gt;, System.Func&lt;T,bool&gt;).T')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Func-2 'System.Func`2')[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Func-2 'System.Func`2')  
  
#### Returns
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')  
