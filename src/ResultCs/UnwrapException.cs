namespace WicalWare.Components.ResultCs;

public class UnwrapException : Exception
{
  private string _message = "Called 'Option.unwrap()' on a 'None' value";
  public override string Message => _message;

  internal UnwrapException() { }

  internal UnwrapException(string errMessage)
  {
    _message = errMessage;
  }
}