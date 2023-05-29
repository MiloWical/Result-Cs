namespace ResultCs.Tests.Unit.Result;

public class ResultMapOrElseTests
{
  private const int k = 21;

  [Fact]
  public void OkMapOrElseTest()
  {
    var x = Result<string, string>.Ok("foo");
    Assert.Equal(x.MapOrElse(_ => k * 2, v => v.Length), 3);
  }

  [Fact]
  public void ErrMapOrElseTest()
  {
    var x = Result<string, string>.Err("bar");
    Assert.Equal(x.MapOrElse(_ => k * 2, v => v.Length), 42);
  }
}