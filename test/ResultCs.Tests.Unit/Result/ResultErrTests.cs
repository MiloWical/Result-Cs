namespace ResultCs.Tests.Unit.Result;

public class ResultErrTests
{
  [Fact]
  public void ErrToNoneTest()
  {
    var x = Result<int, string>.Ok(2);
    Assert.Equal(x.Err(), Option<string>.None());
  }

  [Fact]
  public void ErrToSomeTest()
  {
    const string message = "Nothing here";
    var x = Result<int, string>.Err(message);
    Assert.Equal(x.Err(), Option<string>.Some(message));
  }
}