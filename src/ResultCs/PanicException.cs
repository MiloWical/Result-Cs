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

  /// <summary>
  /// Initializes a new instance of the <see cref="PanicException"/> class.
  /// </summary>
  /// <param name="errMessage">The panic message that the exception contains.</param>
  internal PanicException(string errMessage)
  {
    if (this.message is null)
    {
      throw new PanicException("Message argument passed to the PanicException constructor cannot be null.");
    }

    this.message = errMessage;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PanicException"/> class.
  /// </summary>
  /// <param name="errMessage">The panic message that the exception contains.</param>
  /// <param name="innerException">The underlying exception that generated this panic.</param>
  internal PanicException(string errMessage, Exception innerException)
    : base(errMessage, innerException)
  {
    if (this.message is null)
    {
      throw new PanicException("Message argument passed to the PanicException constructor cannot be null.");
    }

    if (innerException is null)
    {
      throw new PanicException("Inner exception argument passed to the PanicException constructor cannot be null.");
    }

    this.message = $"{errMessage}: {innerException.Message}";
  }

  /// <summary>
  /// Gets the panic message for the exception.
  ///
  /// Overrides the base Object.Message property to return the intended panic message.
  /// </summary>
  public override string Message => this.message;
}