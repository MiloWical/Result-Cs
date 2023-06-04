namespace ResultCs.Tests.Unit.Option;

public class OptionMapTests
{
  [Fact]
  public void OptionMapNoneTest()
  {
    var maybeSomeString = Option<string>.None();
    var maybeSomeLen = maybeSomeString.Map(s => s.Length);

    Assert.Equal(Option<int>.None(), maybeSomeLen);
  }

  [Fact]
  public void OptionMapSomeTest()
  {
    var maybeSomeString = Option<string>.Some("Hello, World!");
    var maybeSomeLen = maybeSomeString.Map(s => s.Length);

    Assert.Equal(Option<int>.Some(13), maybeSomeLen);
  }

  [Fact]
  public void OptionMapNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).Map<string>(null!));
  }

  [Fact]
  public void OptionMapDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).Map<string>(_ => null!));
  }
}