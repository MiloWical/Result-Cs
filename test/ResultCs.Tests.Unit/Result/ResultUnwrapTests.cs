namespace ResultCs.Tests.Unit.Result;

public class ResultUnwrapTests
{
  [Fact]
  public void ResultUnwrapOk()
  {
    const int val = 2;
    var x = Result<int, string>.Ok(val);
    Assert.Equal(val, x.Unwrap());
  }

  [Fact]
  public void ResultUnwrapErr()
  {
    const string err = "emergency failure";
    var x = Result<int, string>.Err(err);
    var thrownException = Assert.Throws<PanicException>(() => x.Unwrap());
    Assert.Equal(err, thrownException.Message);
  }
}