# NOtherLookup

NOtherLookup is a set of LINQ-like extensions that are aimed to make working with [the splendid `ILookup<TKey, TValue>`](http://notherdev.blogspot.com/2014/01/lookup-hidden-gem.html) even easier. It tries to resolve [two main `ILookup`'s deficiencies](http://notherdev.blogspot.com/2014/01/downsides-of-net-lookups.html) - lack of easy way to create it from the scratch and lack of easy way to apply transformations.

## Table of Contents

* [Creating `ILookup`](#creating-ilookup)
  * [Empty `ILookup`](#empty-ilookup)
  * [Creating `ILookup` manually - `Lookup.Builder`](#creating-ilookup-manually---lookupbuilder)
  * [Converting `ILookup` from/to `IDictionary`](#converting-ilookup-fromto-idictionary)
* [Manipulating single `ILookup`](#manipulating-single-ilookup)
  * [`Select` - runs a projection on values for each key](#select---runs-a-projection-on-values-for-each-key)
  * [`Where` - filters values for each key](#where---filters-values-for-each-key)
  * [`OnEachKey` - runs arbitrary LINQ query on lookup elements (`IGrouping`s)](#oneachkey---runs-arbitrary-linq-query-on-lookup-elements-igroupings)
* [Manipulating two `ILookup`s](#manipulating-two-ilookups)
  * [`Concat` - concatenates values for each key](#concat---concatenates-values-for-each-key)
  * [`Union` - gets the unique values for each key](#union---gets-the-unique-values-for-each-key)
  * [`Except` - gets the difference of values set for each key](#except---gets-the-difference-of-values-set-for-each-key)
  * [`Intersect` - gets the intersection of values set for each key](#intersect---gets-the-intersection-of-values-set-for-each-key)
  * [`Join` - combines two lookups by values in each key using provided selector](#join---combines-two-lookups-by-values-in-each-key-using-provided-selector)
  * [`Zip` - combines two lookups by pairs of values using provided selector for each key](#zip---combines-two-lookups-by-pairs-of-values-using-provided-selector-for-each-key)


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

## Manipulating single `ILookup`

Contrary to what we get using standard LINQ operators on `ILookup`, all the operators below maintains `ILookup` typing.

Lookup instance used in the examples:

    ILookup<int, string> lookup = Lookup.Builder
        .WithKey(1, new[] { "a", "b" })
        .WithKey(2, new[] { "c", "d" })
        .Build();
        

### `Select` - runs a projection on values for each key

    ILookup<int, string> projected = lookup.Select(x => x + "!");
    
Result:

    1 => [a!, b!]
    2 => [c!, d!]
    
    
### `Where` - filters values for each key

    ILookup<int, string> filtered = lookup.Select(x => x != "a");
    
Result:

    1 => [b]
    2 => [c, d]
    
    
### `OnEachKey` - runs arbitrary LINQ query on lookup elements (`IGrouping`s)

It is a generalization for any transformation that is supposed to run on each key in lookup. Note that `Select` and `Where` can be also easily accomplished through `OnEachKey` - they are standalone methods only for convenience.

    ILookup<int, string> transformed = lookup.OnEachKey(g => g.Select(x => x + g.Key).Reverse());
    
Result:

    1 => [b1, a1]
    2 => [d2, c2]
    

## Manipulating two `ILookup`s

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

