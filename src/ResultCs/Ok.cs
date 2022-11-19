namespace WicalWare.Components.ResultCs;

public class Ok<T, E> : Result<T, E>
{
  private T _value { get; init; }
  public ResultKind Kind => ResultKind.Ok;
  public Option<E> ErrOption() => new None<E>();

  // I can't completely replicate the Rust syntax because of the follow compiler error:
  // error CS0542: 'Ok': member names cannot be the same as their enclosing type
  public Option<T> OkOption() => new Some<T>(_value);

  public Ok(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    _value = value;
  }

  public T Unwrap() => _value;
}