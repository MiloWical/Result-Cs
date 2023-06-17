using static ResultCs.Tests.Unit.Option.OptionExtensions;

namespace ResultCs.Tests.Unit.Option;

public class OptionComparisonTests
{
  [Theory]
  [InlineData(null, null, true)]
  [InlineData(1, 1, true)]
  [InlineData(null, 1, false)]
  [InlineData(1, null, false)]
  [InlineData(1, 2, false)]
  public void OptionEqualsTest(int? val1, int? val2, bool expectedResult)
  {
    var x = ToOption<int?>(val1);
    var y = ToOption<int?>(val2);
    
    Assert.Equal(expectedResult, x.Equals(y));
  }

  [Fact]
  public void OptionSomeKindReturnValue()
  {
    Option<string> option = Option<string>.Some(string.Empty);

    Assert.Equal(OptionKind.Some, option.Kind);
  }

  [Fact]
  public void OptionSomeUnwrapReturnValue()
  {
    var testString = Guid.NewGuid().ToString();
    Option<string> option = Option<string>.Some(testString);

    Assert.Equal(testString, option.Unwrap());
  }

  [Fact]
  public void OptionSomeNullValueThrowsException()
  {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    var thrownException = Assert.Throws<PanicException>(() => Option<string>.Some(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
  }

  [Fact]
  public void OptionNoneKindReturnValue()
  {
    Option<string> option = Option<string>.None();
    Assert.Equal(OptionKind.None, option.Kind);
  }

  [Fact]
  public void OptionNoneUnwrapThrowsException()
  {
    Option<string> option = Option<string>.None();
    Assert.Throws<PanicException>(() => option.Unwrap());
  }

  [Fact]
  public void OptionSomeIsSomeIsNoneTest()
  {
    var option = Option<string>.Some(string.Empty);

    Assert.True(option.IsSome());
    Assert.False(option.IsNone());
  }

  [Fact]
  public void OptionNoneIsSomeIsNoneTest()
  {
    var option = Option<string>.None();

    Assert.True(option.IsNone());
    Assert.False(option.IsSome());
  }
}