namespace ResultCs.Tests.Unit.Option;

public class OptionOrElseTests
{
  [Fact]
  public void OptionOrElseSomeDelegateReturnsSomeTest()
  {
    Assert.Equal(Option<string>.Some("barbarians"), Option<string>.Some("barbarians").OrElse(Vikings));
  }

  [Fact]
  public void OptionOrElseNoneDelegateReturnsSomeTest()
  {
    Assert.Equal(Option<string>.Some("vikings"), Option<string>.None().OrElse(Vikings));
  }

  [Fact]
  public void OptionOrElseNoneDelegateReturnsNoneTest()
  {
    Assert.Equal(Option<string>.None(), Option<string>.None().OrElse(Nobody));
  }

  [Fact]
  public void OptionOrElseNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().OrElse(null!));
  }

  [Fact]
  public void optionOrElseDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().OrElse(() => null!));
  }

  private Option<string> Nobody() => Option<string>.None();
  private Option<string> Vikings() => Option<string>.Some("vikings");
}