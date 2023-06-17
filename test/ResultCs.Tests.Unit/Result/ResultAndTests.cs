namespace ResultCs.Tests.Unit.Result;

public class ResultAndTests
{
  [Fact]
  public void ResultAndOkTest()
  {
    var x = Result<int, string>.Ok(5);
    var y = Result<int, string>.Ok(2);

    Assert.Equal(x.And<int>(y), Result<int, string>.Ok(2));
  }
  
  [Fact]
  public void ResultAndEarlyErrorTest()
  {
    var x = Result<int, string>.Err("early error");
    var y = Result<string, string>.Ok("foo");
    Assert.Equal(x.And<string>(y), Result<string, string>.Err("early error"));
  }

  [Fact]
  public void ResultAndLateErrorTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<string, string>.Err("late error");
    Assert.Equal(x.And<string>(y), Result<string, string>.Err("late error"));
  }

  [Fact]
  public void ResultAndDoubleErrorTest()
  {
    var x = Result<int, string>.Err("not a 2");
    var y = Result<string, string>.Err("late error");
    Assert.Equal(x.And<string>(y), Result<string, string>.Err("not a 2"));
  }

  [Fact]
  public void ResultAndDifferentReturnTypesTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<string, string>.Ok("different result type");
    Assert.Equal(x.And<string>(y), Result<string, string>.Ok("different result type"));
  }

  [Fact]
  public void ResultAndNullDefaultValueTest()
  {
    Assert.Throws<PanicException>(() => Result<int, string>.Ok(2).And<string>(null!));
  }
}