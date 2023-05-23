namespace WicalWare.Components.ResultCs;

public class PanicException : Exception
{
  private string _message;
  public override string Message => _message;

  internal PanicException(string errMessage)
  {
    _message = errMessage;
  }

  internal PanicException(string errMessage, Exception innerException)
    : base(errMessage, innerException)
  {
    _message = $"{errMessage}: {innerException.Message}";
  }
}