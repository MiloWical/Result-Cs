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

  private int Count(string x)
  {
    return x.Length;
  }
}