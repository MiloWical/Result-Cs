namespace ResultCs.Tests.Unit.Option;

public class OptionGetOrInsertTests
{
  [Fact]
  public void OptionGetOrInsertNoneTest()
  {
    var x = Option<int>.None();
    var y = x.GetOrInsert(5);

    Assert.Equal(5, y);
    Assert.Equal(Option<int>.Some(5), x);
  }

  [Fact]
  public void OptionGetOrInsertSomeTest()
  {
    var x = Option<int>.Some(3);
    var y = x.GetOrInsert(5);

    Assert.Equal(3, y);
    Assert.Equal(Option<int>.Some(3), x);
  }

  [Fact]
  public void OptionGetOrInsertNullValueTest()
  {
    Assert.Throws<PanicException>(() => Option<string>.None().GetOrInsert(null!));
    Assert.Throws<PanicException>(() => Option<string>.Some(string.Empty).GetOrInsert(null!));
  }
}