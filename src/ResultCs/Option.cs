public interface Option<T>
{
  T Unwrap();
  OptionKind Kind { get; }
}