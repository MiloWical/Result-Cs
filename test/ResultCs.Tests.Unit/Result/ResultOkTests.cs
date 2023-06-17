namespace ResultCs.Tests.Unit.Result;

public class ResultOkTests
{
  [Fact]
  public void ResultOkResultOkTest()
  {
    var x = Result<int, string>.Ok(2);
    Assert.Equal(x.Ok(), Option<int>.Some(2));
  }

  [Fact]
  public void ResultErrResultOkTest()
  {
    var x = Result<int, string>.Err("Nothing here");
    Assert.Equal(x.Ok(), Option<int>.None());
  }
}