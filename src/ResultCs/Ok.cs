namespace WicalWare.Components.ResultCs;

public class Ok<T, E> : Result<T, E>
{
  private T _value { get; init; }
  public ResultKind Kind => ResultKind.Ok;
  public Option<E> Err => new None<E>();

  public Ok(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    _value = value;
  }

  public T Unwrap() => _value;
}