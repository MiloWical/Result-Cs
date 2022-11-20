﻿// https://doc.rust-lang.org/std/option/enum.Option.html#
public class Option<T>
{
  // Imp Test Sig
  // ✓        Option<U> And<U>(Option<U> optB)
  //          Option<U> AndThen<U>(Func<Option<U>> f)
  //          T Expect(string message)
  //          Option<T> Filter(Func<T, bool> predicate)
  //          Option<T> Flatten()
  //          T GetOrInsert(T value)
  //          T GetOrInsertWith(Func<T> f)
  //          T Insert(T value)
  // ✓   ✓    bool IsNone()
  // ✓   ✓    bool IsSome()
  //          bool IsSomeAnd(Func<T, bool> f)
  //          IEnumerable<T> Iter()
  //          Option<U> Map<U>(Func<T, U> f)
  //          U MapOr<U>(U default, Func<T, U> f)
  //          U MapOrElse<U>(Func<U> default, Func<T, U> f)
  //          Result<T, E> OkOr<E>(E err)
  //          Result<T, E> OkOrElse<E>(Func<E> err)
  //          Option<T> Or(Option<T> optB)
  //          Option<T> OrElse(Func<Option<T>> f)
  //          Option<T> Replace(T value)
  //          Option<T> Take()
  //          Result<Option<T>, E> Transpose<E>()
  // ✓   ✓    T Unwrap()
  //          T UnwrapOr(T default)
  //          T UnwrapOrDefault()
  //          T UnwrapOrElse(Func<T> f)
  //          Option<T> Xor(Option<T> optB)
  //          Option<(T, U)> Zip<U>(Option<U> other)
  // ✓   ✓    OptionKind Kind { get; }
  //
  // Experimental signatures (not implemented)
  //          bool Contains<U>(U x)
  //          T GetOrInsertDefault()
  //          Option<T> Inspect(Action<T> f)
  //          bool IsSomeAnd(Func<T, bool> f)
  //          (Option<T>, Option<U>) Unzip<U>()
  //          Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> f)

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

  public T UnwrapOr(T def)
  {
    throw new NotImplementedException();
  }

  public T UnwrapOrDefault()
  {
    throw new NotImplementedException();
  }

  public T UnwrapOrElse(Func<T> f)
  {
    throw new NotImplementedException();
  }

  public bool IsSome() => Kind == OptionKind.Some;
  public bool IsNone() => Kind == OptionKind.None;

  public bool IsSomeAnd(Func<T, bool> f)
  {
    throw new NotImplementedException();
  }

  public Option<U> And<U>(Option<U> optB) 
  {
    if (IsNone()) return Option<U>.None();

    return optB;
  }

  public Option<U> AndThen<U>(Func<Option<U>> f)
  {
    throw new NotImplementedException();
  }

  public T Expect(string message)
  {
    throw new NotImplementedException();
  }  

  public Option<T> Filter(Func<T, bool> predicate)
  {
    throw new NotImplementedException();
  }

  public Option<T> Flatten()
  {
    throw new NotImplementedException();
  }

  public T GetOrInsertWith(Func<T> f)
  {
    throw new NotImplementedException();
  }

  public T Insert(T value)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<T> Iter()
  {
    throw new NotImplementedException();
  }

  public Option<U> Map<U>(Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  public U MapOr<U>(U def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  public U MapOrElse<U>(Func<U> def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  public Result<T, E> OkOr<E>(E err)
  {
    throw new NotImplementedException();
  }

  public Result<T, E> OkOrElse<E>(Func<E> err)
  {
    throw new NotImplementedException();
  }

  public Option<T> Or(Option<T> optB)
  {
    throw new NotImplementedException();
  }

  public Option<T> OrElse(Func<Option<T>> f)
  {
    throw new NotImplementedException();
  }

  public Option<T> Replace(T value)
  {
    throw new NotImplementedException();
  }

  public Option<T> Take()
  {
    throw new NotImplementedException();
  }

  public Result<Option<T>, E> Transpose<E>()
  {
    throw new NotImplementedException();
  }

  public Option<T> Xor(Option<T> optB)
  {
    throw new NotImplementedException();
  }

  public Option<(T, U)> Zip<U>(Option<U> other)
  {
    throw new NotImplementedException();
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