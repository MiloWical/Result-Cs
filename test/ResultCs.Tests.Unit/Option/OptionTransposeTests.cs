namespace ResultCs.Tests.Unit.Option;

public class OptionTransposeTests
{
  [Fact]
  public void OptionTransposeSomeOkTest()
  {
    var x = Result<Option<int>, string>.Ok(Option<int>.Some(5));
    var y = Option<Result<int, string>>.Some(Result<int, string>.Ok(5));
    Assert.Equal(x, y.Transpose<int, string>());
  }

  [Fact]
  public void OptionTransposeNoneOkTest()
  {
    var x = Result<Option<int>, string>.Ok(Option<int>.None());
    var y = Option<Result<int, string>>.None();

    Assert.Equal(x, y.Transpose<int, string>());
  }

  [Fact]
  public void OptionTransposeErrOkTest()
  {
    const string err = "Bad mojo";
    var x = Result<Option<int>, string>.Err(err);
    var y = Option<Result<int, string>>.Some(Result<int, string>.Err(err));
    Assert.Equal(x, y.Transpose<int, string>());
  }

  [Fact]
  public void OptionTransposeSomeNonOptionTypeTest()
  {
    Assert.Throws<PanicException>(() => Option<int>.Some(5).Transpose<int, string>());   
  }

  [Fact]
  public void OptionTransposeSomeUnassignableResultOkTypeTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some("5").Transpose<int, string>());   
  }

  [Fact]
  public void OptionTransposeUnassignableResultOkTypeTest()
  {
    Assert.Throws<PanicException>(() => Option<Result<int, int>>.Some(Result<int, int>.Ok(5)).Transpose<string, int>());   
  }

  [Fact]
  public void OptionTransposeUnassignableResultErrTypeTest()
  {
    Assert.Throws<PanicException>(() => Option<Result<int, int>>.Some(Result<int, int>.Ok(5)).Transpose<int, string>());   
  }

  // [Fact]
  // public void OptionTransposeResultErrSomeTest()
  // {
  //   var x = Result<Option<int>, string>.Err("Bad mojo");
  //   var y = Option<Result<int, string>>.Some(Result<int, string>.Err("Bad mojo"));
  //   var transpose = x.Transpose<int>();

  //   Assert.True(y.Equals(transpose));
  // }
}