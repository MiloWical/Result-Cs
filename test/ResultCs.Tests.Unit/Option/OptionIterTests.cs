namespace ResultCs.Tests.Unit.Option;

public class OptionIterTests
{
  [Fact]
  public void OptionIterOkTest()
  {
    var x = Option<int>.Some(7);
    var iter = x.Iter().GetEnumerator();

    Assert.True(iter.MoveNext());
    Assert.Equal(Option<int>.Some(7), iter.Current);
    Assert.False(iter.MoveNext());
  }

  [Fact]
  public void OptionIterNoneTest()
  {
    var x = Option<int>.None();
    var iter = x.Iter().GetEnumerator();

    Assert.True(iter.MoveNext());
    Assert.Equal(iter.Current, Option<int>.None());
    Assert.False(iter.MoveNext());
  }
}