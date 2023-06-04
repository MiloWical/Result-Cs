namespace ResultCs.Tests.Unit.Option;

public class OptionMapOrTests
{
  [Fact]
  public void OptionMapOrSomeTest()
  {
    var x = Option<string>.Some("foo");
    Assert.Equal(3, x.MapOr(42, v => v.Length));
  }

  [Fact]
  public void OptionMapOrNoneTest()
  {
    var x = Option<string>.None();
    Assert.Equal(42, x.MapOr(42, v => v.Length));
  }

  [Fact]
  public void OptionMapOrNullDefaultValueTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().MapOr(null!, _ => "a"));
  }

  [Fact]
  public void OptionMapOrNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).MapOr("a", null!));
  }

  [Fact]
  public void OptionMapOrDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).MapOr("a", _ => null!));
  }
}