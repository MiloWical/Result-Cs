namespace ResultCs.Tests.Unit.Result;

public class ResultTests
{
  [Fact]
  public void ResultOkTest()
  {
    var result = Result<int, string>.Ok(1);

    Assert.True(result.IsOk());
  }

  [Fact]
  public void ResultOkWithNullParamTest()
  {
    Assert.Throws<PanicException>(() => Result<int?, string>.Ok(null));
  }

  [Fact]
  public void ResultErrTest()
  {
    var result = Result<int, string>.Err(string.Empty);

    Assert.True(result.IsErr());
  }

  [Fact]
  public void ResultErrWithNullParamTest()
  {
    Assert.Throws<PanicException>(() => Result<int?, string>.Err(null!));
  }

  [Fact]
  public void ResultOkEqualsTest()
  {
    var x = Result<int, string>.Ok(1);
    var y = Result<int, string>.Ok(1);
    var z = Result<int, string>.Ok(2);

    Assert.Equal(x, y);
    Assert.NotEqual(x, z);
    Assert.NotEqual(y, z);
  }

  [Fact]
  public void ResultErrEqualsTest()
  {
    var x = Result<int, string>.Err(string.Empty);
    var y = Result<int, string>.Err(string.Empty);
    var z = Result<int, string>.Err("Hello, world!");

    Assert.Equal(x, y);
    Assert.Equal(x, z);
    Assert.Equal(y, z);
  }

  [Fact]
  public void ResultOkNotEqualsErrTest()
  {
    var x = Result<int, string>.Ok(1);
    var y = Result<int, string>.Err(string.Empty);

    Assert.NotEqual(x, y);
  }

  [Fact]
  public void ResultIncompatibleTypesTest()
  {
    var x = Result<int, string>.Ok(1);

    Assert.NotEqual(x, (object)1);
  }

  [Fact]
  public void ResultNullEqualsTest()
  {
    var x = Result<int, string>.Ok(1);

    Assert.Throws<PanicException>(() => x.Equals(null));
  }

  [Fact]
  public void ResultOkEqualityOperatorsTest()
  {
    var x = Result<int, string>.Ok(1);
    var y = Result<int, string>.Ok(1);
    var z = Result<int, string>.Ok(2);

    Assert.True(x == y);
    Assert.True(x != z);
    Assert.True(y != z);
  }

  [Fact]
  public void ResultErrEqualityOperatorsTest()
  {
    var x = Result<int, string>.Err(string.Empty);
    var y = Result<int, string>.Err(string.Empty);
    var z = Result<int, string>.Err("Hello, world!");

    Assert.True(x == y);
    Assert.True(x == z);
    Assert.True(y == z);
  }

  [Fact]
  public void ResultOkHashCodeTest()
  {
    var x = Result<int, string>.Ok(1);
    var y = Result<int, string>.Ok(1);

    Assert.Equal(x.GetHashCode(), x.GetHashCode());
    Assert.Equal(y.GetHashCode(), y.GetHashCode());
    Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
  }

  [Fact]
  public void ResultErrHashCodeTest()
  {
    var x = Result<int, string>.Err(string.Empty);
    var y = Result<int, string>.Err(string.Empty);

    Assert.Equal(x.GetHashCode(), x.GetHashCode());
    Assert.Equal(y.GetHashCode(), y.GetHashCode());
    Assert.NotEqual(x.GetHashCode(), y.GetHashCode());
  }

  [Fact]
  public void ResultEqualityOperatorLeftOperandNullTest()
  {
    Assert.Throws<PanicException>(() => null! == Result<string, string>.Ok(string.Empty));
  }

  [Fact]
  public void ResultEqualityOperatorRightOperandNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty) == null!);
  }

  [Fact]
  public void ResultInequalityOperatorLeftOperandNullTest()
  {
    Assert.Throws<PanicException>(() => null! != Result<string, string>.Ok(string.Empty));
  }

  [Fact]
  public void ResultInequalityOperatorRightOperandNullTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty) != null!);
  }
}