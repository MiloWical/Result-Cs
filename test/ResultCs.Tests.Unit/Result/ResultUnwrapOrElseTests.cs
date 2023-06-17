namespace ResultCs.Tests.Unit.Result;

public class ResultUnwrapOrElseTests
{
  [Fact]
  public void ResultUnwrapOrElseOkTest()
  {
    Assert.Equal(2, Result<int, string>.Ok(2).UnwrapOrElse(Count));
  }

  [Fact]
  public void ResultUnwrapOrElseErrTest()
  {
    Assert.Equal(3, Result<int, string>.Err("foo").UnwrapOrElse(Count));
  }

  [Fact]
  public void ResultUnwrapOrElseNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Err("foo").UnwrapOrElse(null!));
  }

  [Fact]
  public void ResultUnwrapOrElseDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Err("foo").UnwrapOrElse(_ => null!));
  }

  private int Count(string x)
  {
    return x.Length;
  }
}