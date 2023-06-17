namespace ResultCs.Tests.Unit.Result;

public class ResultOrElseTests
{
  [Fact]
  public void ResultOkChainResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Ok(2).OrElse(sq).OrElse(sq), Result<int, int>.Ok(2));
  }

  [Fact]
  public void ResultErrOkResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Ok(2).OrElse(err).OrElse(sq), Result<int, int>.Ok(2));
  }

  [Fact]
  public void ResultOkErrResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Err(3).OrElse(sq).OrElse(err), Result<int, int>.Ok(9));
  }

  [Fact]
  public void ResultErrChainResultOrElseTest()
  {
    Assert.Equal(Result<int, int>.Err(3).OrElse(err).OrElse(err), Result<int, int>.Err(3));
  }

  [Fact]
  public void ResultOrElseNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<int, int>.Err(1).OrElse<string>(null!));
  }

  [Fact]
  public void ResultOrElseDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<int, int>.Err(1).OrElse<string>(_ => null!));
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