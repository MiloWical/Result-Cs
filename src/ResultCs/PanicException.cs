// Copyright (c) Milo Wical. All rights reserved.

namespace WicalWare.Components.ResultCs;

/// <summary>
/// Represents an erroneous condition encountered in <c>Options</c>s and <c>Result</c>s.
/// 
/// It is intended to reflect the panic semantics in Rust.
/// </summary>
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

  /// <summary>
  /// Gets the panic message for the exception.
  /// 
  /// Overrides the base Object.Message property to return the intended panic message.
  /// </summary>
  public override string Message => this.message;
}