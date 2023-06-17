namespace ResultCs.Tests.Unit.Result;

public class ResultMapOrElseTests
{
  private const int k = 21;

  [Fact]
  public void ResultMapOrElseOkResultTest()
  {
    var x = Result<string, string>.Ok("foo");
    Assert.Equal(3, x.MapOrElse(_ => k * 2, v => v.Length));
  }

  [Fact]
  public void ResultMapOrElseErrResultTest()
  {
    var x = Result<string, string>.Err("bar");
    Assert.Equal(42, x.MapOrElse(_ => k * 2, v => v.Length));
  }

  [Fact]
  public void ResultMapOrElseNullDefaultDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOrElse(null!, _ => string.Empty));
  }

  [Fact]
  public void ResultMapOrElseNullMappingDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOrElse(_ => string.Empty, null!));
  }

  [Fact]
  public void ResultMapOrElseAllNullParamsTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOrElse<string>(null!, null!));
  }

  [Fact]
  public void ResultMapOrElseDefaultDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Err(string.Empty).MapOrElse(_ => null!, _ => string.Empty));
  }

  [Fact]
  public void ResultMapOrElseMappingDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).MapOrElse(_ => string.Empty, _ => null!));
  }
}