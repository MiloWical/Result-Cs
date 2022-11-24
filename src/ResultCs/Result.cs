namespace WicalWare.Components.ResultCs;

// https://doc.rust-lang.org/std/result/enum.Result.html
public class Result<T, E>
{
  // Imp Test Sig
  //          Result<U, E> And<U>(Result<U, E> optB)
  //          Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  //          
  //
  // Experimental signatures (not implemented)
  //          bool Contains<U>(U x)

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
}