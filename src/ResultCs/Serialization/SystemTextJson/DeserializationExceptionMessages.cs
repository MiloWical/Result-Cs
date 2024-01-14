// Copyright (c) Milo Wical. All rights reserved.

namespace WicalWare.Components.ResultCs.Serialization.SystemTextJson;

/// <summary>
/// Exception messages thrown during deserialization.
/// </summary>
public class DeserializationExceptionMessages
{
  /// <summary>
  /// Exception message thrown when the string doesn't have a start object token.
  /// </summary>
  public const string ExpectingStartObjectToken = "Expecting start object token.";

  /// <summary>
  /// Exception message thrown when deserializing an Option.Some with a null value.
  /// </summary>
  public const string NullOptionSomeValue = "Cannot create an Option.Some with a null value.";

  /// <summary>
  /// Exception message thrown when deserializing a Result.Ok with a null value.
  /// </summary>
  public const string NullResultOkValue = "Cannot create a Result.Ok with a null value.";

  /// <summary>
  /// Exception message thrown when deserializing a Result.Err with a null value.
  /// </summary>
  public const string NullResultErrValue = "Cannot create a Result.Err with a null value.";

  /// <summary>
  /// Exception message thrown when deserializing an Option with an illegal kind type.
  /// </summary>
  public const string IllegalOptionKindValue = "Could not deserialize an Option with the specified kind.";

   /// <summary>
  /// Exception message thrown when deserializing Result with an illegal kind type.
  /// </summary>
  public const string IllegalResultKindValue = "Could not deserialize a Result with the specified kind.";
}