namespace ResultCs.Tests.Unit.Option;

public class OptionOkOrTests
{
  [Fact]
  public void OptionOkOrSomeTest()
  {
    var x = Option<string>.Some("foo");
    Assert.Equal(Result<string, int>.Ok("foo"), x.OkOr(0));
  }

  [Fact]
  public void OptionOkOrNoneTest()
  {
    var x = Option<string>.None();
    Assert.Equal(Result<string, int>.Err(0), x.OkOr(0));
  }

  [Fact]
  public void OptionOkOrNullErrTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().OkOr<string>(null!));
  }
}