namespace ResultCs.Tests.Unit.Option;

public class OptionExpectTests
{
  [Fact]
  public void OptionExpectSomeTest()
  {
    var x = Option<string>.Some("value");
    Assert.Equal("value", x.Expect("fruits are healthy"));
  }

  [Fact]
  public void OptionExpectNoneTest()
  {
    const string message = "fruits are healthy";

    var x = Option<string>.None();
    
    var thrownException = Assert.Throws<PanicException>(() => x.Expect(message)); // panics with <c>fruits are healthy<c>
    Assert.Equal(message, thrownException.Message);
  }
}