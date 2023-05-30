namespace ResultCs.Tests.Unit.Option;

internal static class OptionExtensions
{
  internal static Option<T> ToOption<T>(T val)
  {
    return val == null ? Option<T>.None() : Option<T>.Some(val);
  }
}