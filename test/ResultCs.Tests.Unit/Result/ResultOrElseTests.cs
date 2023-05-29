namespace ResultCs.Tests.Unit.Result;

public class ResultOrElseTests
{
  [Fact]
  public void OkChainResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Ok(2).OrElse(sq).OrElse(sq), Result<int, int>.Ok(2));
  }

  [Fact]
  public void ErrOkResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Ok(2).OrElse(err).OrElse(sq), Result<int, int>.Ok(2));
  }

  [Fact]
  public void OkErrResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Err(3).OrElse(sq).OrElse(err), Result<int, int>.Ok(9));
  }

  [Fact]
  public void ErrChainResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Err(3).OrElse(err).OrElse(err), Result<int, int>.Err(3));
  }

  private Result<int, int> sq(int x) 
  {
    return Result<int, int>.Ok(x * x); 
  }
  
  private Result<int, int> err(int x)
  {
    return Result<int, int>.Err(x);
  }
}