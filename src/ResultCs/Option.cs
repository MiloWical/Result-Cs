public class Option<T>
{
  // Imp Test Sig
  // ✓        Option<U> And<U>(Option<U> optB);
  //          Option<U> AndThen<U>(Func<Option<U>> f);
  //          T Expect(string message);
  //          Option<T> Filter(Func<T, bool> predicate);
  //          Option<T> Flatten();
  //          T GetOrInsert();
  //          T GetOrInsertDefault();
  //          T GetOrInsertWith(Func<T> f);
  //          T Insert(T value);
  // ✓   ✓    bool IsNone();
  // ✓   ✓    bool IsSome();
  //          bool IsSomeAnd(Func<T, bool> f);
  //          IEnumerable<T> Iter();
  //          Option<U> Map<U>(Func<T, U> f);

  // CONTINUE IMPLEMENTING FROM MAP!!!

  // ✓   ✓    T Unwrap();
  // ✓   ✓    OptionKind Kind { get; }

  public OptionKind Kind { get; private set; }
  private T? _value;

  public static Option<T> Some(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    var option = new Option<T>();
    option._value = value;
    option.Kind = OptionKind.Some;

    return option;
  }

  public static Option<T> None()
  {
    return new Option<T>
    {
      Kind = OptionKind.None
    };
  }

  public T Unwrap()
  {
    if (IsNone()) throw new UnwrapException();

    return _value!;
  }

  public bool IsSome() => Kind == OptionKind.Some;
  public bool IsNone() => Kind == OptionKind.None;

  public Option<U> And<U>(Option<U> optB) 
  {
    if (IsNone()) return Option<U>.None();

    return optB;
  }
}

#if TEST
public class OptionTests
{
  [Fact]
  public void SomeKindReturnValue()
  {
    Option<string> option = Option<string>.Some(string.Empty);

    Assert.Equal(OptionKind.Some, option.Kind);
  }

  [Fact]
  public void SomeUnwrapReturnValue()
  {
    var testString = Guid.NewGuid().ToString();
    Option<string> option = Option<string>.Some(testString);

    Assert.Equal(testString, option.Unwrap());
  }

  [Fact]
  public void SomeNullValueThrowsException()
  {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    var thrownException = Assert.Throws<ArgumentNullException>(() => Option<string>.Some(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    Assert.Equal("value", thrownException.ParamName);
  }

  [Fact]
  public void NoneKindReturnValue()
  {
    Option<string> option = Option<string>.None();
    Assert.Equal(OptionKind.None, option.Kind);
  }

  [Fact]
  public void NoneUnwrapThrowsException()
  {
    Option<string> option = Option<string>.None();
    Assert.Throws<UnwrapException>(() => option.Unwrap());
  }

  [Fact]
  public void SomeIsSomeIsNoneTest()
  {
    var option = Option<string>.Some(string.Empty);

    Assert.True(option.IsSome());
    Assert.False(option.IsNone());
  }

  [Fact]
  public void NoneIsSomeIsNoneTest()
  {
    var option = Option<string>.None();

    Assert.True(option.IsNone());
    Assert.False(option.IsSome());
  }
}
#endif