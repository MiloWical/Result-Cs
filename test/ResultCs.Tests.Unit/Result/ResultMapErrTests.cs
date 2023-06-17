namespace ResultCs.Tests.Unit.Result;

public class ResultMapErrTests
{
  [Fact]
  public void ResultMapErrOkTest()
  {
    var x = Result<int, int>.Ok(2);
    Assert.Equal(x.MapErr(Stringify), Result<int, string>.Ok(2));
  }

  [Fact]
  public void ResultMapErrErrTest()
  {
    var x = Result<int, int>.Err(13);
    Assert.Equal(x.MapErr(Stringify), Result<int, string>.Err("error code: 13"));
  }
  [Fact]
  public void ResultMapErrNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<int, int>.Err(1).MapErr<int>(null!));
  }

  [Fact]
  public void ResultMapErrDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<int, int>.Err(1).MapErr<int?>(_ => null!));
  }

  private string Stringify(int x)
  {
    return $"error code: {x}";
  }
}