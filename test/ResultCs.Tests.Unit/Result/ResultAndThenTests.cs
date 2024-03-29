namespace ResultCs.Tests.Unit.Result;

public class ResultAndThenTests
{
  [Fact]
  public void ResultAndThenOkTest()
  {
    Assert.Equal(Result<int, string>.Ok(2).AndThen(SquareThenToString), Result<string, string>.Ok(4.ToString()));
  }

  [Fact]
  public void ResultAndThenOkExceptionTest()
  {
    Assert.Equal(Result<int, string>.Ok(int.MaxValue).AndThen(SquareThenToString), Result<string, string>.Err("overflowed"));
  }

  [Fact]
  public void ResultAndThenErrTest()
  {
    Assert.Equal(Result<int, string>.Err("not a number").AndThen(SquareThenToString), Result<string, string>.Err("not a number"));
  }

  private Result<string, string> SquareThenToString(int x)
  {
    try
    {
      int product;
      
      checked
      {
        product = x*x;
      }

      return Result<string, string>.Ok(product.ToString());
    }
    catch(OverflowException)
    {
      return Result<string, string>.Err("overflowed");
    }
  }

  [Fact]
  public void ResultAndThenNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).AndThen<string>(null!));
  }

  [Fact]
  public void ResultAndThenDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).AndThen<string>(_ => null!));
  }
}