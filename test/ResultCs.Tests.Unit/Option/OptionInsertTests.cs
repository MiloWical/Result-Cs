namespace ResultCs.Tests.Unit.Option;

public class OptionInsertTests
{
  [Fact]
  public void OptionInsertNoneTest()
  {
    var opt = Option<int>.None();
    var val = opt.Insert(1);
    Assert.Equal(1, val);
    Assert.Equal(1, opt.Unwrap());
  }

  [Fact]
  public void OptionInsertSomeTest()
  {
    var opt = Option<int>.Some(1);
    var val = opt.Insert(2);
    Assert.Equal(2, val);
    Assert.Equal(Option<int>.Some(2), opt);
  }

  [Fact]
  public void OptionInsertNullValueTest()
  {
    Assert.Throws<ArgumentNullException>(() => Option<string>.None().Insert(null!));
    Assert.Throws<ArgumentNullException>(() => Option<string>.Some(string.Empty).Insert(null!));
  }
}