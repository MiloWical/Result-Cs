namespace ResultCs.Tests.Unit.Option;

public class OptionMapOrElseTests
{
  const int k = 21;

  [Fact]
  public void OptionMapOrElseSomeTest()
  {
    var x = Option<string>.Some("foo");
    Assert.Equal(3, x.MapOrElse(() => 2 * k, v => v.Length));
  }

  [Fact]
  public void OptionMapOrElseNoneTest()
  {
    var x = Option<string>.None();
    Assert.Equal(42, x.MapOrElse(() => 2 * k, v => v.Length));
  }

  [Fact]
  public void OptionMapOrElseNullDefaultDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().MapOrElse(null!, _ => "a"));
  }

  [Fact]
  public void OptionMapOrElseDefaultDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().MapOrElse(() => null!, _ => "a"));
  }

  [Fact]
  public void OptionMapOrElseNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).MapOrElse(() => "a", null!));
  }

  [Fact]
  public void OptionMapOrElseDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).MapOrElse(() => "a", _ => null!));
  }
}