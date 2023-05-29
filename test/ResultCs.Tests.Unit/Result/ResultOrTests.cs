namespace ResultCs.Tests.Unit.Result;

public class ResultOrTests
{
  [Fact]
  public void OkErrResultOrTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<int, string>.Err("late error");
    Assert.Equal(x.Or(y), Result<int, string>.Ok(2));
  }

  [Fact]
  public void ErrOkResultOrTest()
  {
    var x = Result<int, string>.Err("early error");
    var y = Result<int, string>.Ok(2);
    Assert.Equal(x.Or(y), Result<int, string>.Ok(2));
  }

  [Fact]
  public void ErrErrResultOrTest()
  {
    var x = Result<int, string>.Err("not a 2");
    var y = Result<int, string>.Err("late error");
    Assert.Equal(x.Or(y), Result<int, string>.Err("late error"));
  }

  [Fact]
  public void OkOkResultOrTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<int, string>.Ok(100);
    Assert.Equal(x.Or(y), Result<int, string>.Ok(2));
  }
}