namespace WicalWare.Components.ResultCs;

public class UnwrapException : Exception
{
  public override string Message => "Called 'Option.unwrap()' on a 'None' value";
}