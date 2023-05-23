namespace ResultCs.Tests.Unit.Result;

public class ResultIsOkTests
{
  [Fact]
  public void IsOkForErrTest()
  {
    Assert.False(Result<string, string>.Err(string.Empty).IsOk());
  }

  [Fact]
  public void IsOkForOkTest()
  {
    Assert.True(Result<string, string>.Ok(string.Empty).IsOk());
  }
}