namespace WicalWare.Components.ResultCs;

// https://doc.rust-lang.org/std/result/enum.Result.html
public class Result<T, E>
{
  // Imp Test Sig
  //          Result<U, E> And<U>(Result<U, E> optB)
  //          Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  //          Option<E> Err()
  //          T Expect(string msg)
  //          E ExpectErr(string msg)
  //          bool IsErr()
  //          bool IsOk()
  //          IEnumerable<Option<T>> Iter()
  //          Result<U, E> Map<U>(Func<T, U> op)
  //          Result<T, F> MapErr<F>(Func<E, F> op)
  //          U MapOr<U>(U def, Func<T, U> f)
  //          U MapOrElse<U>(Func<E, U> def, Func<T, U> f)
  //          Option<T> Ok()
  //          Result<T, F> Or<F>(Result<T, F> res)
  //          Result<T, F> OrElse<F>(Func<E, Result<T, F>> op) 
  //          Option<Result<T, E>> Transpose()
  //          T Unwrap()
  //          E UnwrapErr()
  //          T UnwrapOr(T def)
  //          T UnwrapOrDefault()
  //          T UnwrapOrElse(Func<E, T> op)
  //
  // Experimental signatures (not implemented)
  //          bool Contains<U>(U x)
  //          bool ContainsErr<F>(F f)
  //          Result<T, E> Flatten()
  //          Result<T, E> Inspect(Action<T> f)
  //          Result<T, E> InspectErr(Action<E> f)
  //          E IntoErr()
  //          T IntoOk()
  //          bool IsErrAnd(Func<E, bool> f)
  //          bool IsOkAnd(Func<T, bool> f)


  public ResultKind Kind { get; private set; }
  private T? _value;
  private E? _err;
  
  public static Result<T, E> Ok(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    var result = new Result<T, E>();
    result._value = value;
    result.Kind = ResultKind.Ok;

    return result;
  }

  public static Result<T, E> Err(E err)
  {
    ArgumentNullException.ThrowIfNull(err);

    var result = new Result<T, E>();
    result._err = err;
    result.Kind = ResultKind.Err;

    return result;
  }

  // TODO: Fix this up for this type!

  // public override bool Equals(object? other)
  // {
  //   if(other == null) ArgumentNullException.ThrowIfNull(other);

  //   var otherOption = (Option<T>) other;

  //   if(this.Kind != otherOption.Kind)
  //     return false;

  //   if(this.IsNone() && otherOption.IsNone())
  //     return true;

  //   if(_value == null)
  //   {
  //     if(otherOption.Unwrap() == null)
  //       return true;

  //     return false;
  //   }

  //   if(_value.Equals(otherOption.Unwrap()))
  //     return true;

  //   return false;
  // }

  // public static bool operator == (Option<T> opt1, Option<T> opt2) => opt1.Equals(opt2);

  // public static bool operator != (Option<T> opt1, Option<T> opt2) => !(opt1.Equals(opt2));

  // public override int GetHashCode() => ((object) this).GetHashCode();

  public Result<U, E> And<U>(Result<U, E> optB)
  {
    throw new NotImplementedException();
  }
  
  public Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  {
    throw new NotImplementedException();
  } 

  public Option<E> Err()
  {
    throw new NotImplementedException();
  }
  
  public T Expect(string msg)
  {
    throw new NotImplementedException();
  }
  
  public E ExpectErr(string msg)
  {
    throw new NotImplementedException();
  }
  
  public bool IsErr()
  {
    throw new NotImplementedException();
  }
  
  public bool IsOk()
  {
    throw new NotImplementedException();
  }
  
  public IEnumerable<Option<T>> Iter()
  {
    throw new NotImplementedException();
  }
  
  public Result<U, E> Map<U>(Func<T, U> op)
  {
    throw new NotImplementedException();
  }
  
  public Result<T, F> MapErr<F>(Func<E, F> op)
  {
    throw new NotImplementedException();
  }
  
  public U MapOr<U>(U def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }
  
  public U MapOrElse<U>(Func<E, U> def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }
  
  public Option<T> Ok()
  {
    throw new NotImplementedException();
  }
  
  public Result<T, F> Or<F>(Result<T, F> res)
  {
    throw new NotImplementedException();
  }
  
  public Result<T, F> OrElse<F>(Func<E, Result<T, F>> op)
  {
    throw new NotImplementedException();
  }
  
  public Option<Result<T, E>> Transpose()
  {
    throw new NotImplementedException();
  }
  
  public T Unwrap()
  {
    throw new NotImplementedException();
  }
  
  public E UnwrapErr()
  {
    throw new NotImplementedException();
  }
  
  public T UnwrapOr(T def)
  {
    throw new NotImplementedException();
  }
  
  public T UnwrapOrDefault()
  {
    throw new NotImplementedException();
  }
  
  public T UnwrapOrElse(Func<E, T> op)
  {
    throw new NotImplementedException();
  }
}

#if TEST
public class ResultTests
{
  [Fact]
  public void DummyTest()
  {
    Assert.True(true);
  }
}
#endif