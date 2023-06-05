namespace ResultCs.Tests.Unit.Option;

public class OptionReplaceTests
{
  [Fact]
  public void OptionReplaceSomeSomeTest()
  {
    var x = Option<int>.Some(2);
    var old = x.Replace(5);
    Assert.Equal(Option<int>.Some(5), x);
    Assert.Equal(Option<int>.Some(2), old);
  }

  [Fact]
  public void OptionReplaceNoneSomeTest()
  {
    var x = Option<int>.None();
    var old = x.Replace(3);
    Assert.Equal(Option<int>.Some(3), x);
    Assert.Equal(Option<int>.None(), old);
  }

  [Fact]
  public void OptionReplaceNullValueTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().Replace(null!));
  }
}