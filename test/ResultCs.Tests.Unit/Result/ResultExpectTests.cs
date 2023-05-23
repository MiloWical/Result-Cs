namespace ResultCs.Tests.Unit.Result;

public class ResultExpectTests
{
  const string okMessage = "Response message";
  const string errMessage = "Bad mojo";
  const string errorMessage = "Something wrong";

  [Fact]
  public void ExpectOkTest()
  {
    Assert.Equal(okMessage, Result<string, string>.Ok(okMessage).Expect(errorMessage));
  }

  [Fact]
  public void ExpectErrStringTest()
  {
    PanicException response = Assert.Throws<PanicException>(() => Result<string, string>.Err(errMessage).Expect(errorMessage));

    Assert.Contains(errMessage, response.Message);
    Assert.Contains(errorMessage, response.Message);
  }

  [Fact]
  public void ExpectErrExceptionTest()
  {
    PanicException response = Assert.Throws<PanicException>(() => Result<string, Exception>.Err(new Exception(errMessage)).Expect(errorMessage));

    Assert.IsType<Exception>(response.InnerException);
    Assert.Contains(errMessage, response.Message);
    Assert.Contains(errorMessage, response.Message);
  }
}