# NOtherLookup

NOtherLookup is a set of LINQ-like extensions that are aimed to make working with [the splendid `ILookup<TKey, TValue>`](http://notherdev.blogspot.com/2014/01/lookup-hidden-gem.html) even easier. It tries to resolve [two main `ILookup`'s deficiencies](http://notherdev.blogspot.com/2014/01/downsides-of-net-lookups.html) - lack of easy way to create it from the scratch and lack of easy way to apply transformations.

## Creating `ILookup`

.NET's [`Lookup`](http://msdn.microsoft.com/en-us/library/bb460184\(v=vs.110\).aspx) class has no public constructor. NOtherLookup offers several ways to obtain an `ILookup<TKey, TValue>` instance.

### Empty `ILookup`

Useful to keep the code clean and obvious.

    ILookup<int, string> emptyLookup = Lookup.Empty<int, string>();

### Creating `ILookup` manually - `Lookup.Builder`

    ILookup<int, string> lookup = Lookup.Builder
        .WithKey(1, new[] { "a", "b" })
        .WithKey(2, new[] { "c", "d" })
        .Build();

Allows specifying a custom key comparer:

    ILookup<int, string> lookup = Lookup.Builder
        .WithComparer(new CustomComparer())
        .WithKey(1, new[] { "a", "b" })
        .WithKey(2, new[] { "c", "d" })
        .Build();

### Converting `ILookup` from/to `IDictionary`

    IDictionary<int, string[]> sourceDictionary = new Dictionary<int, string[]>()
    {
      { 1, new[] { "a", "b" }},
      { 2, new[] { "c", "d" }}
    };

Converting dictionaries to lookups works for multiple types of `TValue` collections - `TValue[]`, `ICollection<TValue>` and `IList<TValue>` - as well as for `IEnumerable<IGrouping<TKey, TValue>>` (decomposed lookup).

    ILookup<int, string> lookup = sourceDictionary.ToLookup();
    
And back to mutable `IDictionary` - doable using standard LINQ operators, but quite verbose and convoluted.
    
    Dictionary<int, List<string>> backToDict = lookup.ToDictionary();

## Operations on `ILookup`

Contrary to what we get using standard LINQ operators on `ILookup`, all the operators below maintains `ILookup` typing.

The operators allow specifying custom key comparers, where applicable.

Lookup instances used in the examples:

    ILookup<int, string> first = Lookup.Builder
        .WithKey(1, new[] { "a", "b" })
        .WithKey(2, new[] { "c", "d" })
        .Build();
        
    ILookup<int, string> second = Lookup.Builder
        .WithKey(1, new[] { "a", "c" })
        .WithKey(3, new[] { "e", "f" })
        .Build();
      


### `Select` - runs a projection on values for each key

    ILookup<int, string> projected = first.Select(x => x + "!");
    
Result:

    1 => [a!, b!]
    2 => [c!, d!]
    
    
### `Where` - filters values for each key

    ILookup<int, string> filtered = first.Select(x => x != "a");
    
Result:

    1 => [b]
    2 => [c, d]
    
    
### `Concat` - concatenates values for each key

    ILookup<int, string> concatenated = first.Concat(second);
    
Result:

    1 => [a, b, a, c]
    2 => [c, d]
    3 => [e, f]
 
    
### `Union` - gets the unique values for each key

    ILookup<int, string> unionized = first.Union(second);
    
Result:

    1 => [a, b, c]
    2 => [c, d]
    3 => [e, f]
    
Also supports custom values comparers.


### `Except` - gets the difference of values set for each key

    ILookup<int, string> difference = first.Except(second);
    
Result:

    1 => [b]
    2 => [c, d]
    
Also supports custom values comparers.


### `Intersect` - gets the intersection of values set for each key

    ILookup<int, string> intersection = first.Intersect(second);
    
Result:

    1 => [a]
    
Also supports custom values comparers.


### `Join` - combines two lookups by values in each key using provided selector

    ILookup<int, string> joined = first.Join(second, (x, y) => x + y);
    
Result:

    1 => [aa, ac, ba, bc]
    
    
### `Zip` - combines two lookups by pairs of values using provided selector for each key

    ILookup<int, string> zipped = first.Zip(second, (x, y) => x + y);
    
Result:

    1 => [aa, bc]

