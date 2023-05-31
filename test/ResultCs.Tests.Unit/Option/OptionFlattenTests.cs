namespace ResultCs.Tests.Unit.Option;

public class OptionFlattenTests
{
  [Fact]
  public void OptionFlattenSomeSomeTest()
  {
    var x = Option<Option<int>>.Some(Option<int>.Some(6));
    Assert.Equal(Option<int>.Some(6), x.Flatten());
  }

  [Fact]
  public void OptionFlattenSomeNoneTest()
  {
    var x = Option<Option<int>>.Some(Option<int>.None());
    Assert.Equal(Option<int>.None(), x.Flatten());
  }

  [Fact]
  public void OptionFlattenNoneTest()
  {
    var x = Option<Option<int>>.None();
    Assert.Equal(Option<int>.None(), x.Flatten());
  }

  [Fact]
  public void OptionFlattenMultipleDimensionsTest()
  {
    var x = Option<Option<Option<int>>>.Some(Option<Option<int>>.Some(Option<int>.Some(6)));
    Assert.Equal(Option<Option<int>>.Some(Option<int>.Some(6)), x.Flatten());
    Assert.Equal(Option<int>.Some(6), x.Flatten().Flatten());
  }
}