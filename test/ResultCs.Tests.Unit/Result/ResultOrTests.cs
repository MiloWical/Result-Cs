namespace ResultCs.Tests.Unit.Result;

public class ResultOrTests
{
  [Fact]
  public void ResultOkErrResultOrTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<int, string>.Err("late error");
    Assert.Equal(x.Or(y), Result<int, string>.Ok(2));
  }

  [Fact]
  public void ResultErrOkResultOrTest()
  {
    var x = Result<int, string>.Err("early error");
    var y = Result<int, string>.Ok(2);
    Assert.Equal(x.Or(y), Result<int, string>.Ok(2));
  }

  [Fact]
  public void ResultErrErrResultOrTest()
  {
    var x = Result<int, string>.Err("not a 2");
    var y = Result<int, string>.Err("late error");
    Assert.Equal(x.Or(y), Result<int, string>.Err("late error"));
  }

  [Fact]
  public void ResultOkOkResultOrTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<int, string>.Ok(100);
    Assert.Equal(x.Or(y), Result<int, string>.Ok(2));
  }

  [Fact]
  public void ResultOrNullDefaultTest()
  {
    Assert.Throws<PanicException>(() => Result<int, string>.Ok(2).Or<int>(null!));
  }
}