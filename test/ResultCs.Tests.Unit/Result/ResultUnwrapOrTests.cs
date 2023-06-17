namespace ResultCs.Tests.Unit.Result;

public class ResultUnwrapOrTests
{
  private const int def = 2;

  [Fact]
  public void ResultUnwrapOrOkTest()
  {
    var x = Result<int, string>.Ok(9);
    Assert.Equal(9, x.UnwrapOr(def));
  }

  [Fact]
  public void ResultUnwrapOrErrTest()
  {
    var x = Result<int, string>.Err("error");
    Assert.Equal(def, x.UnwrapOr(def));
  }

  [Fact]
  public void ResultUnwrapOrNullDefaultTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Err("error").UnwrapOr(null!));
  }
}