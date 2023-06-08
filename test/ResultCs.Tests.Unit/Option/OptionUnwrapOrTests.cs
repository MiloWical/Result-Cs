namespace ResultCs.Tests.Unit.Option;

public class OptionUnwrapOrTests
{
  [Fact]
  public void OptionUnwrapOrSomeTest()
  {
    Assert.Equal("car", Option<string>.Some("car").UnwrapOr("bike"));
  }

  [Fact]
  public void OptionUnwrapOrNoneTest()
  {
    Assert.Equal("bike", Option<string>.None().UnwrapOr("bike"));
  }

  [Fact]
  public void OptionUnwrapOrNullDefaultValueTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().UnwrapOr(null!));
  }
}