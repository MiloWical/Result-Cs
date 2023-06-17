namespace ResultCs.Tests.Unit.Result;

public class ResultUnwrapOrDefaultTests
{
  [Fact]
  public void ResultUnwrapOrDefaultOkTest()
  {
    var goodYearFromInput = "1909";
    var goodYear = Parse(goodYearFromInput).UnwrapOrDefault();

    Assert.Equal(1909, goodYear);
  }

  [Fact]
  public void ResultUnwrapOrDefaultErrTest()
  {
    var badYearFromInput = "190blarg";
    var badYear = Parse(badYearFromInput).UnwrapOrDefault();

    Assert.Equal(0, badYear);
  }

  [Fact]
  public void ResultUnwrapOrDefaultNullTypeDefaultTest()
  {
    Assert.Throws<PanicException>(() => Result<string, string>.Err(string.Empty).UnwrapOrDefault());
  }

  private Result<int, string> Parse(string input)
  {
    if(int.TryParse(input, out int parsedInput))
      return Result<int, string>.Ok(parsedInput);

    return Result<int, string>.Err($"Could not parse input: '{input}'");
  }
}