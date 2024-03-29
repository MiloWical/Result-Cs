namespace ResultCs.Tests.Unit.Result;

public class ResultExpectTests
{
  const string okMessage = "Response message";
  const string errMessage = "Bad mojo";
  const string panicMessage = "Something wrong";

  [Fact]
  public void ResultExpectOkTest()
  {
    Assert.Equal(okMessage, Result<string, string>.Ok(okMessage).Expect(panicMessage));
  }

  [Fact]
  public void ResultExpectErrStringTest()
  {
    PanicException response = Assert.Throws<PanicException>(() => Result<string, string>.Err(errMessage).Expect(panicMessage));

    Assert.Contains(errMessage, response.Message);
    Assert.Contains(panicMessage, response.Message);
  }

  [Fact]
  public void ResultExpectErrExceptionTest()
  {
    PanicException response = Assert.Throws<PanicException>(() => Result<string, Exception>.Err(new Exception(errMessage)).Expect(panicMessage));

    Assert.IsType<Exception>(response.InnerException);
    Assert.Contains(errMessage, response.Message);
    Assert.Contains(panicMessage, response.Message);
  }

  [Fact]
  public void ResultExpectNullMessageTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Ok(string.Empty).Expect(null!));
  }
}