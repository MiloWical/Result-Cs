namespace ResultCs.Tests.Unit.Result;

public class ResultTransposeTests
{
  [Fact]
  public void ResultTransposeOkSomeTest()
  {
    var x = Result<Option<int>, string>.Ok(Option<int>.Some(5));
    var y = Option<Result<int, string>>.Some(Result<int, string>.Ok(5));
    var transpose = x.Transpose();
  }
}