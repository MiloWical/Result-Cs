namespace ResultCs.Tests.Unit.Option;

public class OptionOkOrElseTests
{
  [Fact]
  public void OptionOkOrElseSomeTest()
  {
    var x = Option<string>.Some("foo");
    Assert.Equal(Result<string, int>.Ok("foo"), x.OkOrElse(() => 0));
  }

  [Fact]
  public void OptionOkOrElseNoneTest()
  {
    var x = Option<string>.None();
    Assert.Equal(Result<string, int>.Err(0), x.OkOrElse(() => 0));
  }

  [Fact]
  public void OptionOkOrElseNullErrDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().OkOrElse<string>(null!));
  }

  [Fact]
  public void OptionOkOrElseErrDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().OkOrElse<string>(() => null!));
  }
}