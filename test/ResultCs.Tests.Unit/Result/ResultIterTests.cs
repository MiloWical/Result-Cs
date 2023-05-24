namespace ResultCs.Tests.Unit.Result;

public class ResultIterTests
{
  [Fact]
  public void IterOkTest()
  {
    var x = Result<int, string>.Ok(7);
    var iter = x.Iter().GetEnumerator();

    Assert.True(iter.MoveNext());
    Assert.Equal(iter.Current, Option<int>.Some(7));
    Assert.False(iter.MoveNext());
  }

  [Fact]
  public void IterErrTest()
  {
    var x = Result<int, string>.Err("nothing!");
    var iter = x.Iter().GetEnumerator();

    Assert.True(iter.MoveNext());
    Assert.Equal(iter.Current, Option<int>.None());
    Assert.False(iter.MoveNext());
  }
}