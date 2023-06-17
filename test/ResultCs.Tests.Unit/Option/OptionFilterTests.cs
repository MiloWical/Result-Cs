namespace ResultCs.Tests.Unit.Option;

public class OptionFilterTests
{
  [Fact]
  public void OptionFilterSomeTruePredicateTest()
  {
    Assert.Equal(Option<int>.Some(4), Option<int>.Some(4).Filter(IsEven));
  }

  [Fact]
  public void OptionFilterSomeFalsePredicateTest()
  {
    Assert.Equal(Option<int>.None(), Option<int>.Some(3).Filter(IsEven));
  }

  [Fact]
  public void OptionFilterNoneTest()
  {
    Assert.Equal(Option<int>.None(), Option<int>.None().Filter(IsEven));
  }

  [Fact]
  public void OptionFilterNullPredicateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).Filter(null!));
  }

  private bool IsEven(int n)
  {
    return (n % 2) == 0;
  }
}