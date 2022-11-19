namespace WicalWare.Components.ResultCs;

public class Some<T> : Option<T>
{
  public OptionKind Kind => OptionKind.Some;
  private T _value { get; init; }

  public Some(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    _value = value;
  }

  public T Unwrap() => _value;
}

#if TEST
public class SomeTests
{
  [Fact]
  public void KindReturnValue()
  {
    Option<string> option = new Some<string>(string.Empty);

    Assert.Equal(OptionKind.Some, option.Kind);
  }

  [Fact]
  public void UnwrapReturnValue()
  {
    var testString = Guid.NewGuid().ToString();
    Option<string> option = new Some<string>(testString);

    Assert.Equal(testString, option.Unwrap());
  }

  [Fact]
  public void NullValueThrowsException()
  {
    var thrownException = Assert.Throws<ArgumentNullException>(() => new Some<string>(null));
    Assert.Equal("value", thrownException.ParamName);
  }
}
#endif