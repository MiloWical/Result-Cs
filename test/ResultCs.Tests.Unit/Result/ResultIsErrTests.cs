namespace ResultCs.Tests.Unit.Result;

public class ResultIsErrTests
{
  [Fact]
  public void ResultIsErrForErrTest()
  {
    Assert.True(Result<string, string>.Err(string.Empty).IsErr());
  }

  [Fact]
  public void ResultIsErrForOkTest()
  {
    Assert.False(Result<string, string>.Ok(string.Empty).IsErr());
  }
}