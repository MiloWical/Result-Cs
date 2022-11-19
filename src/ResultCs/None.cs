namespace WicalWare.Components.ResultCs;

public class None<T> : Option<T>
{
  public OptionKind Kind => OptionKind.None;

  public T Unwrap() => throw new UnwrapException();
}

#if TEST
public class NoneTests
{
  [Fact]
  public void KindReturnValue()
  {
    Option<string> option = new None<string>();
    Assert.Equal(OptionKind.None, option.Kind);
  }

  [Fact]
  public void UnwrapThrowsException()
  {
    Option<string> option = new None<string>();
    Assert.Throws<UnwrapException>(() => option.Unwrap());
  }
}
#endif