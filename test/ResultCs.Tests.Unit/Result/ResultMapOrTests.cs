namespace ResultCs.Tests.Unit.Result;

public class ResultMapOrTests
{
  [Fact]
  public void OkMapOrTest()
  {
    var x = Result<string, string>.Ok("foo");
    Assert.Equal(3, x.MapOr(42, v => v.Length));
  }

  [Fact]
  public void ErrMapOrTest()
  {
    var x = Result<string, string>.Err("bar");
    Assert.Equal(42, x.MapOr(42, v => v.Length));
  }
}