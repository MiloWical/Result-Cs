// Copyright (c) Milo Wical. All rights reserved.

namespace WicalWare.Components.ResultCs;

/// <summary>
/// An enum that helps identify if the <c>Result</c> contains a value or an error.
/// </summary>
public enum ResultKind
{
  /// <summary>
  /// Indicates that the <c>Result</c> contains a value.
  /// </summary>
  Ok,

  /// <summary>
  /// Indicates that the <c>Result</c> contains an error.
  /// </summary>
  Err,
}