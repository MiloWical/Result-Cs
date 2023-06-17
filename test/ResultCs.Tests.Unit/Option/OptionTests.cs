namespace ResultCs.Tests.Unit.Option;

public class OptionTests
{
  [Fact]
  public void OptionEqualityOperatorTest()
  {
    var v = Option<int>.None();
    var w = Option<int>.None();
    var x = Option<int>.Some(1);
    var y = Option<int>.Some(1);
    var z = Option<int>.Some(2);

    Assert.True(v == w);
    Assert.True(x == y);
    Assert.False(w == x);
    Assert.False(y == z);
  }

  [Fact]
  public void OptionEqualityOperatorNullLeftParamTest()
  {
    var x = Option<int>.Some(1);

    Assert.Throws<PanicException>(() => null! == x);
  }

  [Fact]
  public void OptionEqualityOperatorNullRightParamTest()
  {
    var x = Option<int>.Some(1);

    Assert.Throws<PanicException>(() => x == null!);
  }

  [Fact]
  public void OptionNotEqualOperatorTest()
  {
    var v = Option<int>.None();
    var w = Option<int>.None();
    var x = Option<int>.Some(1);
    var y = Option<int>.Some(1);
    var z = Option<int>.Some(2);

    Assert.False(v != w);
    Assert.False(x != y);
    Assert.True(w != x);
    Assert.True(y != z);
  }

  [Fact]
  public void OptionNotEqualOperatorNullLeftParamTest()
  {
    var x = Option<int>.Some(1);

    Assert.Throws<PanicException>(() => null! != x);
  }

  [Fact]
  public void OptionNotEqualOperatorNullRightParamTest()
  {
    var x = Option<int>.Some(1);

    Assert.Throws<PanicException>(() => x != null!);
  }

  [Fact]
  public void OptionEqualsNullParamTest()
  {
    var x = Option<int>.Some(1);
    
    Assert.Throws<PanicException>(() => x.Equals(null!));
  }

  [Fact]
  public void OptionEqualsSomeValueNullTest()
  {
    var opt = new Option<string>();

    Assert.Throws<PanicException>(() => opt.Equals(Option<string>.Some(string.Empty)));
  }

  [Fact]
  public void OptionGetHashCodeTest()
  {
    var w = Option<int>.None();
    var x = Option<int>.None();
    var y = Option<int>.Some(1);
    var z = Option<int>.Some(1);

    Assert.Equal(w.GetHashCode(), w.GetHashCode());
    Assert.NotEqual(w.GetHashCode(), x.GetHashCode());

    Assert.Equal(y.GetHashCode(), y.GetHashCode());
    Assert.NotEqual(y.GetHashCode(), z.GetHashCode());
  }
}