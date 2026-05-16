using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;
using Godot.Collections;

namespace NS;

// TODO: Add tests for Dictionary when it supports the spread operator (potentially coming in .NET 10)

[GodotClass]
public partial class NodeWithSpeciallyRecognizedMarshalling : Node
{
    [BindProperty]
    public int[] ArrayOfInts { get; set; }

    [BindMethod]
    public void MethodThatTakesArrayOfInts(int[] array) { }

    [BindMethod]
    public int[] MethodThatReturnsArrayOfInts() => [];

    [BindProperty]
    public List<int> ListOfInts { get; set; }

    [BindMethod]
    public void MethodThatTakesListOfInts(List<int> list) { }

    [BindMethod]
    public List<int> MethodThatReturnsListOfInts() => [];

    [BindProperty]
    public ImmutableArray<int> ImmutableArrayOfInts { get; set; }

    [BindProperty]
    public IReadOnlyList<int> IReadOnlyListOfInts { get; set; }

    [BindMethod]
    public void MethodThatTakesImmutableArrayOfInts(ImmutableArray<int> array) { }

    [BindMethod]
    public void MethodThatTakesIListOfInts(IList<int> list) { }

    [BindMethod]
    public ImmutableArray<int> MethodThatReturnsImmutableArrayOfInts() => [];

    [BindMethod]
    public IEnumerable<int> MethodThatReturnsIEnumerableOfInts() => [];

    [BindProperty]
    public IReadOnlyCollection<int> IReadOnlyCollectionOfInts { get; set; }

    [BindMethod]
    public void MethodThatTakesICollectionOfInts(ICollection<int> collection) { }

    [BindMethod]
    public Collection<int> MethodThatReturnsCollectionOfInts() => [];

    [BindMethod]
    public ReadOnlyCollection<int> MethodThatReturnsReadOnlyCollectionOfInts() => [];

    [BindProperty]
    public bool[] ArrayOfBooleans { get; set; }

    [BindMethod]
    public void MethodThatTakesArrayOfBooleans(bool[] array) { }

    [BindMethod]
    public bool[] MethodThatReturnsArrayOfBooleans() => [];

    [BindProperty]
    public List<bool> ListOfBooleans { get; set; }

    [BindMethod]
    public void MethodThatTakesListOfBooleans(List<bool> list) { }

    [BindMethod]
    public List<bool> MethodThatReturnsListOfBooleans() => [];

    [BindProperty]
    public ImmutableArray<bool> ImmutableArrayOfBooleans { get; set; }

    [BindMethod]
    public void MethodThatTakesImmutableArrayOfBooleans(ImmutableArray<bool> array) { }

    [BindMethod]
    public ImmutableArray<bool> MethodThatReturnsImmutableArrayOfBooleans() => [];
}
