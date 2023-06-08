namespace ResultCs.Tests.Unit.Option;

public class OptionUnwrapOrElseTests
{
  const int k = 10;

  [Fact]
  public void OptionUnwrapOrElseSomeTest()
  {
    Assert.Equal(4, Option<int>.Some(4).UnwrapOrElse(() => 2 * k));
  }
  
  [Fact]
  public void OptionUnwrapOrElseNoneTest()
  {
    Assert.Equal(20, Option<int>.None().UnwrapOrElse(() => 2 * k));
  }

  [Fact]
  public void OptionUnwrapOrElseNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().UnwrapOrElse(null!));
  }

  [Fact]
  public void OptionUnwrapOrElseDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().UnwrapOrElse(() => null!));
  }
}