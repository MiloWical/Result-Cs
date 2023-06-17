namespace ResultCs.Tests.Unit.Result;

public class ResultTransposeTests
{
  [Fact]
  public void ResultTransposeOkSomeTest()
  {
    var x = Result<Option<int>, string>.Ok(Option<int>.Some(5));
    var y = Option<Result<int, string>>.Some(Result<int, string>.Ok(5));
    var transpose = x.Transpose<int>();

    Assert.True(y.Equals(transpose));
  }

  [Fact]
  public void ResultTransposeOkNoneTest()
  {
    var x = Result<Option<int>, string>.Ok(Option<int>.None());
    var y = Option<Result<int, string>>.None();
    var transpose = x.Transpose<int>();

    Assert.True(y.Equals(transpose));
  }

  [Fact]
  public void ResultTransposeOkNonOptionTypeTest()
  {
    Assert.Throws<PanicException>(() => Result<int, string>.Ok(5).Transpose<int>());   
  }

  [Fact]
  public void ResultTransposeOkUnassignableTypeTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok("5").Transpose<int>());   
  }

  [Fact]
  public void ResultTransposeErrSomeTest()
  {
    var x = Result<Option<int>, string>.Err("Bad mojo");
    var y = Option<Result<int, string>>.Some(Result<int, string>.Err("Bad mojo"));
    var transpose = x.Transpose<int>();

    Assert.True(y.Equals(transpose));
  }

  [Fact]
  public void ResultTransposeTypeMismatchTest()
  {
    var res = Result<Option<string>, string>.Ok(Option<string>.Some("value"));

    Assert.Throws<PanicException>(() => res.Transpose<int>());
  }
}