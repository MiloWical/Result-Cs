namespace ResultCs.Tests.Unit.Option;

public class OptionIsSomeAndTests
{
  [Fact]
  public void OptionIsSomeAndTruePredicateTest()
  {
    var x = Option<int>.Some(2);
    Assert.True(x.IsSomeAnd(x => x > 1));
  }

  [Fact]
  public void OptionIsSomeAndFalsePredicateTest()
  {
    var x = Option<int>.Some(0);
    Assert.False(x.IsSomeAnd(x => x > 1));
  }

  [Fact]
  public void OptionIsSomeAndNoneTest()
  {
    var x = Option<int>.None();
    Assert.False(x.IsSomeAnd(x => x > 1));
  }
}