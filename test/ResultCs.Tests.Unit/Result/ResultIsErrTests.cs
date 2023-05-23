namespace ResultCs.Tests.Unit.Result;

public class ResultIsErrTests
{
  [Fact]
  public void IsErrForErrTest()
  {
    Assert.True(Result<string, string>.Err(string.Empty).IsErr());
  }

  [Fact]
  public void IsErrForOkTest()
  {
    Assert.False(Result<string, string>.Ok(string.Empty).IsErr());
  }
}