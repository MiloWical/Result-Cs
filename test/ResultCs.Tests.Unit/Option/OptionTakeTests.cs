namespace ResultCs.Tests.Unit.Option;

public class OptionTakeTests
{
  [Fact]
  public void optionTakeSomeTest()
  {
    var x = Option<int>.Some(2);
    var y = x.Take();
    Assert.Equal(Option<int>.None(), x);
    Assert.Equal(Option<int>.Some(2), y);
  }

  [Fact]
  public void OptionTakeNoneTest()
  {
    var x = Option<int>.None();
    var y = x.Take();
    Assert.Equal(Option<int>.None(), x);
    Assert.Equal(Option<int>.None(), y);
  }
}