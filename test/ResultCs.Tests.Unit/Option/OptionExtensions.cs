namespace ResultCs.Tests.Unit.Option;

public static class OptionExtensions
{
  public static Option<T> ToOption<T>(T val)
  {
    return val == null ? Option<T>.None() : Option<T>.Some(val);
  }
}