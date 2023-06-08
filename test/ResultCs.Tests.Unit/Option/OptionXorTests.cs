namespace ResultCs.Tests.Unit.Option;

public class OptionXorTests
{
  [Fact]
  public void OptionXorSomeNoneTest()
  {
    var x = Option<int>.Some(2);
    var y = Option<int>.None();
    Assert.Equal(Option<int>.Some(2), x.Xor(y));
  }

  [Fact]
  public void OptionXorNoneSomeTest()
  {
    var x = Option<int>.None();
    var y = Option<int>.Some(2);
    Assert.Equal(Option<int>.Some(2), x.Xor(y));
  }

  [Fact]
  public void OptionXorSomeSomeTest()
  {
    var x = Option<int>.Some(2);
    var y = Option<int>.Some(2);
    Assert.Equal(Option<int>.None(), x.Xor(y));
  }

  [Fact]
  public void OptionXorNoneNoneTest()
  {
    var x = Option<int>.None();
    var y = Option<int>.None();
    Assert.Equal(Option<int>.None(), x.Xor(y));
  }

  [Fact]
  public void OptionXorNullOptBTest()
  {
    Assert.Throws<PanicException>(() => Option<int>.None().Xor(null!));
  }
}