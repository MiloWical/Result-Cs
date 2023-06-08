// Copyright (c) Milo Wical. All rights reserved.

namespace WicalWare.Components.ResultCs;

public class PanicException : Exception
{
  private string message;

  internal PanicException(string errMessage)
  {
    this.message = errMessage;
  }

  internal PanicException(string errMessage, Exception innerException)
    : base(errMessage, innerException)
  {
    this.message = $"{errMessage}: {innerException.Message}";
  }

  public override string Message => this.message;
}