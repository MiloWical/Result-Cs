namespace ResultCs.Tests.Unit.Result;

public class ResultIsOkTests
{
  [Fact]
  public void ResultIsOkForErrTest()
  {
    Assert.False(Result<string, string>.Err(string.Empty).IsOk());
  }

  [Fact]
  public void ResultIsOkForOkTest()
  {
    Assert.True(Result<string, string>.Ok(string.Empty).IsOk());
  }
}