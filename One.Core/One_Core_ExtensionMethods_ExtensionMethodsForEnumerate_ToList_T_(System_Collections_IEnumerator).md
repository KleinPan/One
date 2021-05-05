#### [One.Core](index.md 'index')
### [One.Core.ExtensionMethods](One_Core_ExtensionMethods.md 'One.Core.ExtensionMethods').[ExtensionMethodsForEnumerate](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate.md 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate')
## ExtensionMethodsForEnumerate.ToList&lt;T&gt;(IEnumerator) Method
Adds each item in the [System.Collections.IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.IEnumerator 'System.Collections.IEnumerator') into a [System.Collections.Generic.List&lt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1') and return the new [System.Collections.Generic.List&lt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1'). 
```csharp
public static System.Collections.Generic.List<T> ToList<T>(this System.Collections.IEnumerator enumerator);
```
#### Type parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ToList_T_(System_Collections_IEnumerator)_T'></a>
`T`  
The type of the elements in the [System.Collections.Generic.List&lt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1'). 
  
#### Parameters
<a name='One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ToList_T_(System_Collections_IEnumerator)_enumerator'></a>
`enumerator` [System.Collections.IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.IEnumerator 'System.Collections.IEnumerator')  
The [System.Collections.IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.IEnumerator 'System.Collections.IEnumerator') instance that the extension method affects. 
  
#### Returns
[System.Collections.Generic.List&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')[T](One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ToList_T_(System_Collections_IEnumerator).md#One_Core_ExtensionMethods_ExtensionMethodsForEnumerate_ToList_T_(System_Collections_IEnumerator)_T 'One.Core.ExtensionMethods.ExtensionMethodsForEnumerate.ToList&lt;T&gt;(System.Collections.IEnumerator).T')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')  
The [System.Collections.Generic.List&lt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1') instance with the elements of the [System.Collections.IEnumerator](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.IEnumerator 'System.Collections.IEnumerator'). 
