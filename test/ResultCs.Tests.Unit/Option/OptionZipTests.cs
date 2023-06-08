namespace ResultCs.Tests.Unit.Option;

public class OptionZipTests
{
  [Fact]
  public void OptionZipSomeSomeTest()
  {
    var x = Option<int>.Some(1);
    var y = Option<string>.Some("hi");

    Assert.Equal(Option<(int, string)>.Some((1, "hi")), x.Zip(y));
    Assert.Equal(Option<(string, int)>.Some(("hi", 1)), y.Zip(x));
  }

  [Fact]
  public void OptionZipSomeNoneTest()
  {
    var x = Option<int>.Some(1);
    var z = Option<short>.None();

    Assert.Equal(Option<(int, short)>.None(), x.Zip(z));
  }

  [Fact]
  public void OptionZipNoneSomeTest()
  {
    var y = Option<string>.Some("hi");
    var z = Option<short>.None();

    Assert.Equal(Option<(short, string)>.None(), z.Zip(y));
  }

  [Fact]
  public void OptionZipNullParamTest()
  {
    Assert.Throws<PanicException>(() => Option<int>.None().Zip<string>(null!));
  }
}