using static ResultCs.Tests.Unit.Option.OptionExtensions;
namespace ResultCs.Tests.Unit.Option;

public class OptionAndTests
{
  [Theory]
  [InlineData(null, null, null)]
  [InlineData(2, null, null)]
  [InlineData(null, 2, null)]
  [InlineData(1, 2, 2)]
  [InlineData("foo", 2, 2)]
  [InlineData(2, "bar", "bar")]
  [InlineData("foo", null, null)]
  public void AndTests(object? val1, object? val2, object? expectedValue)
  {
    var x = ToOption<object?>(val1);
    var y = ToOption<object?>(val2);
    var expectedResult = ToOption<object?>(expectedValue);

    Assert.Equal(expectedResult, x.And(y));
  }

  [Fact]
  public void AndMistmatchedTypeTest()
  {
    var x = Option<int>.Some(2);
    var y = Option<string>.Some("foo");
    var z = Option<bool>.None();

    Assert.IsType<string>(x.And<string>(y).Unwrap());
    Assert.IsType<int>(y.And<int>(x).Unwrap());

    Assert.True(x.And<bool>(z).IsNone());
    Assert.True(y.And<bool>(z).IsNone());
    Assert.True(z.And<int>(x).IsNone());
    Assert.True(z.And<string>(y).IsNone());
  }
}