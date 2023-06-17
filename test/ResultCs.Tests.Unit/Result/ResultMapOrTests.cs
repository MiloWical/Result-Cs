namespace ResultCs.Tests.Unit.Result;

public class ResultMapOrTests
{
  [Fact]
  public void ResultMapOrOkResultTest()
  {
    var x = Result<string, string>.Ok("foo");
    Assert.Equal(3, x.MapOr(42, v => v.Length));
  }

  [Fact]
  public void ResultMapOrErrResultTest()
  {
    var x = Result<string, string>.Err("bar");
    Assert.Equal(42, x.MapOr(42, v => v.Length));
  }

  [Fact]
  public void ResultMapOrNullDefaultTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOr(null!, _ => string.Empty));
  }

  [Fact]
  public void ResultMapOrNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOr(string.Empty, null!));
  }

  [Fact]
  public void ResultMapOrAllNullParamsTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOr<string>(null!, null!));
  }

  [Fact]
  public void ResultMapOrDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOr(string.Empty, _ => null!));
  }
}