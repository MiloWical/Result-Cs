namespace ResultCs.Tests.Unit.Result;

public class ResultUnwrapErrTests
{
  [Fact]
  public void ResultUnwrapErrOkTest()
  {
    const string err = "emergency failure";
    var x = Result<int, string>.Err(err);
    
    Assert.Equal(err, x.UnwrapErr());
  }

  [Fact]
  public void ResultUnwrapErrErrTest()
  {
    const int val = 2;
    var x = Result<int, string>.Ok(val);
    var thrownException = Assert.Throws<PanicException>(() => x.UnwrapErr());
    Assert.Equal(val.ToString(), thrownException.Message);
  }
}