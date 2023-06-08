namespace ResultCs.Tests.Unit.Option;

public class OptionUnwrapOrDefaultTests
{
  [Fact]
  public void OptionUnwrapOrDefaultSomeTest()
  {
    var goodYearFromInput = "1909";
    var goodYear = Parse(goodYearFromInput).UnwrapOrDefault();
    Assert.Equal(1909, goodYear);
  }

  [Fact]
  public void OptionUnwrapOrDefaultNoneTest()
  {
    var badYearFromInput = "190blarg";
    var badYear = Parse(badYearFromInput).UnwrapOrDefault();
    Assert.Equal(0, badYear);
  }

  [Fact]
  public void OptionUnwrapOrDefaultNullDefaultTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().UnwrapOrDefault());
  }

  private Option<int> Parse(string input)
  {
    if(int.TryParse(input, out var output))
    {
      return Option<int>.Some(output);
    }

    return Option<int>.None();
  }
}