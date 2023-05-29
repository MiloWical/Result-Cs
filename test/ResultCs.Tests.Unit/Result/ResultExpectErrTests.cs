namespace ResultCs.Tests.Unit.Result;

public class ResultExpectErrTests
{
  const int okValue = 10;
  const string errMessage = "Bad mojo";
  const string panicMessage = "Something wrong";

  [Fact]
  public void ExpectErrWithErrTest()
  {
    var output = Result<int, string>.Err(errMessage).ExpectErr(panicMessage);

    Assert.Equal(errMessage, output);
  }

  [Fact]
  public void ExpectErrWithOkTest()
  {
    var output = Assert.Throws<PanicException>(() => Result<int, string>.Ok(okValue).ExpectErr(panicMessage));

    Assert.Contains(okValue.ToString(), output.Message);
    Assert.Contains(panicMessage, output.Message);
  }
}