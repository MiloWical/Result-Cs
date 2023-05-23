namespace ResultCs.Tests.Unit.Result;

public class ResultTests
{
  [Fact]
  public void OkTest()
  {
    var result = Result<int, string>.Ok(1);

    Assert.True(result.IsOk());
  }

  [Fact]
  public void OkWithNullParamTest()
  {
    Assert.Throws<ArgumentNullException>(() => Result<int?, string>.Ok(null));
  }

  [Fact]
  public void ErrTest()
  {
    var result = Result<int, string>.Err(string.Empty);

    Assert.True(result.IsErr());
  }

  [Fact]
  public void ErrWithNullParamTest()
  {
    Assert.Throws<ArgumentNullException>(() => Result<int?, string>.Err(null!));
  }

  [Fact]
  public void OkEqualsTest()
  {
    var x = Result<int, string>.Ok(1);
    var y = Result<int, string>.Ok(1);
    var z = Result<int, string>.Ok(2);

    Assert.Equal(x, y);
    Assert.NotEqual(x, z);
    Assert.NotEqual(y, z);
  }

  [Fact]
  public void ErrEqualsTest()
  {
    var x = Result<int, string>.Err(string.Empty);
    var y = Result<int, string>.Err(string.Empty);
    var z = Result<int, string>.Err("Hello, world!");

    Assert.Equal(x, y);
    Assert.Equal(x, z);
    Assert.Equal(y, z);
  }

  [Fact]
  public void OkNotEqualsErrTest()
  {
    var x = Result<int, string>.Ok(1);
    var y = Result<int, string>.Err(string.Empty);

    Assert.NotEqual(x, y);
  }
}