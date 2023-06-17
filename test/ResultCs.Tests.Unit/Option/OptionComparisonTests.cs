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
  public void EqualsTest(int? val1, int? val2, bool expectedResult)
  {
    var x = ToOption<int?>(val1);
    var y = ToOption<int?>(val2);
    
    Assert.Equal(expectedResult, x.Equals(y));
  }

  [Fact]
  public void SomeKindReturnValue()
  {
    Option<string> option = Option<string>.Some(string.Empty);

    Assert.Equal(OptionKind.Some, option.Kind);
  }

  [Fact]
  public void SomeUnwrapReturnValue()
  {
    var testString = Guid.NewGuid().ToString();
    Option<string> option = Option<string>.Some(testString);

    Assert.Equal(testString, option.Unwrap());
  }

  [Fact]
  public void SomeNullValueThrowsException()
  {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    var thrownException = Assert.Throws<PanicException>(() => Option<string>.Some(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
  }

  [Fact]
  public void NoneKindReturnValue()
  {
    Option<string> option = Option<string>.None();
    Assert.Equal(OptionKind.None, option.Kind);
  }

  [Fact]
  public void NoneUnwrapThrowsException()
  {
    Option<string> option = Option<string>.None();
    Assert.Throws<PanicException>(() => option.Unwrap());
  }

  [Fact]
  public void SomeIsSomeIsNoneTest()
  {
    var option = Option<string>.Some(string.Empty);

    Assert.True(option.IsSome());
    Assert.False(option.IsNone());
  }

  [Fact]
  public void NoneIsSomeIsNoneTest()
  {
    var option = Option<string>.None();

    Assert.True(option.IsNone());
    Assert.False(option.IsSome());
  }
}