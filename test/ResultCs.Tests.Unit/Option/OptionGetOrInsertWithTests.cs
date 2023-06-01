namespace ResultCs.Tests.Unit.Option;

public class OptionGetOrInsertWithTests
{
  [Fact]
  public void OptionGetOrInsertWithNoneTest()
  {
    var x = Option<int>.None();
    var y = x.GetOrInsertWith(() => 5);

    Assert.Equal(5, y);
    Assert.Equal(Option<int>.Some(5), x);
  }

  [Fact]
  public void OptionGetOrInsertWithSomeTest()
  {
    var x = Option<int>.Some(3);
    var y = x.GetOrInsertWith(() => 5);

    Assert.Equal(3, y);
    Assert.Equal(Option<int>.Some(3), x);
  }

  [Fact]
  public void OptionGetOrInsertWithNullDelegateTest()
  {
    Assert.Throws<ArgumentNullException>(() => Option<string>.None().GetOrInsertWith(null!));
    Assert.Throws<ArgumentNullException>(() => Option<string>.Some(string.Empty).GetOrInsertWith(null!));
  }

  [Fact]
  public void OptionGetOrInsertWithDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().GetOrInsertWith(() => null!));
  }
}