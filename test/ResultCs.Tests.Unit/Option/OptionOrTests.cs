namespace ResultCs.Tests.Unit.Option;

public class OptionOrTests
{
  [Fact]
  public void OptionOrSomeNoneTest()
  {
    var x = Option<int>.Some(2);
    var y = Option<int>.None();
    Assert.Equal(Option<int>.Some(2), x.Or(y));
  }

  [Fact]
  public void OptionOrNoneSomeTest()
  {
    var x = Option<int>.None();
    var y = Option<int>.Some(100);
    Assert.Equal(Option<int>.Some(100), x.Or(y));
  }

  [Fact]
  public void OptionOrSomeSomeTest()
  {
    var x = Option<int>.Some(2);
    var y = Option<int>.Some(100);
    Assert.Equal(Option<int>.Some(2), x.Or(y));
  }

  [Fact]
  public void OptionOrNoneNoneTest()
  {
    var x = Option<int>.None();
    var y = Option<int>.None();
    Assert.Equal(Option<int>.None(), x.Or(y));
  }

  [Fact]
  public void OptionOrNullAlternateOptionTest()
  {
    Assert.Throws<PanicException>(() => Option<int>.None().Or(null!));
  }
}