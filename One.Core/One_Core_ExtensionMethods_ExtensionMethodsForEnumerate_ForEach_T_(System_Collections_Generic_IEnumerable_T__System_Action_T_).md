#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForEnumerate](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate.md 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate')
## ExtensionMethodsForEnumerate.ForEach&lt;T&gt;(IEnumerable&lt;T&gt;, Action&lt;T&gt;) Method
Enumerates for each in this collection. 
```csharp
public static System.Collections.Generic.IEnumerable<T> ForEach<T>(this System.Collections.Generic.IEnumerable<T> @this, System.Action<T> action);
```
#### Type parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_)_T'></a>
`T`  
Generic type parameter. 
  
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_)_this'></a>
`this` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[T](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_).md#One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_)_T 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate.ForEach&lt;T&gt;(System.Collections.Generic.IEnumerable&lt;T&gt;, System.Action&lt;T&gt;).T')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')  
The @this to act on. 
  
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_)_action'></a>
`action` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-1 'System.Action`1')[T](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_).md#One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_)_T 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate.ForEach&lt;T&gt;(System.Collections.Generic.IEnumerable&lt;T&gt;, System.Action&lt;T&gt;).T')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-1 'System.Action`1')  
The action. 
  
#### Returns
[System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[T](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_).md#One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ForEach_T_(System_Collections_Generic_IEnumerable_T__System_Action_T_)_T 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate.ForEach&lt;T&gt;(System.Collections.Generic.IEnumerable&lt;T&gt;, System.Action&lt;T&gt;).T')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')  
An enumerator that allows foreach to be used to process for each in this collection. 
