namespace WicalWare.Components.ResultCs;

public interface Result<T, E>
{
  ResultKind Kind { get; }
  T Unwrap();
  Option<E> ErrOption();
  Option<T> OkOption();
}