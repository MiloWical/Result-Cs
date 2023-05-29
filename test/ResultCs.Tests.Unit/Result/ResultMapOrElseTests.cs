namespace ResultCs.Tests.Unit.Result;

public class ResultMapOrElseTests
{
  private const int k = 21;

  [Fact]
  public void OkResultMapOrElseTest()
  {
    var x = Result<string, string>.Ok("foo");
    Assert.Equal(3, x.MapOrElse(_ => k * 2, v => v.Length));
  }

  [Fact]
  public void ErrResultMapOrElseTest()
  {
    var x = Result<string, string>.Err("bar");
    Assert.Equal(42, x.MapOrElse(_ => k * 2, v => v.Length));
  }
}