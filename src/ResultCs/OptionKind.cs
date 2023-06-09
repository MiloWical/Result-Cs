// Copyright (c) Milo Wical. All rights reserved.

namespace WicalWare.Components.ResultCs;

/// <summary>
/// An enum that helps identify if the <c>Option</c> has a value or not.
/// </summary>
public enum OptionKind
{
  /// <summary>
  /// Indicates that the <c>Option</c> has a value.
  /// </summary>
  Some,

  /// <summary>
  /// Indicates that the <c>Option</c> does not have a value.
  /// </summary>
  None,
}