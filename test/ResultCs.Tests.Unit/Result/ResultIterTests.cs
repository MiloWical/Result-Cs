namespace ResultCs.Tests.Unit.Result;

public class ResultIterTests
{
  [Fact]
  public void ResultIterOkTest()
  {
    var x = Result<int, string>.Ok(7);
    var iter = x.Iter().GetEnumerator();

    Assert.True(iter.MoveNext());
    Assert.Equal(Option<int>.Some(7), iter.Current);
    Assert.False(iter.MoveNext());
  }

  [Fact]
  public void ResultIterErrTest()
  {
    var x = Result<int, string>.Err("nothing!");
    var iter = x.Iter().GetEnumerator();

    Assert.True(iter.MoveNext());
    Assert.Equal(Option<int>.None(), iter.Current);
    Assert.False(iter.MoveNext());
  }
}