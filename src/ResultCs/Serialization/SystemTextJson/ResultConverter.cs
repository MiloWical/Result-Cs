// Copyright (c) Milo Wical. All rights reserved.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace WicalWare.Components.ResultCs.Serialization.SystemTextJson;

/// <summary>
/// A converter for System.Text.Json that handles serializing
/// Result{TOk, TErr} values.
/// </summary>
/// <typeparam name="TOk">The underlying Ok type of the Result.</typeparam>
/// <typeparam name="TErr">The underlying Err type of the Result.</typeparam>
public class ResultConverter<TOk, TErr> : JsonConverter<Result<TOk, TErr>>
{
  /// <inheritdoc />
  public override Result<TOk, TErr>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    ResultKind kind = (ResultKind)(-1);
    TOk? val = default;
    TErr? err = default;

    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    reader.Read();

    while (reader.TokenType != JsonTokenType.EndObject)
    {
      if (reader.TokenType != JsonTokenType.PropertyName)
      {
        throw new JsonException();
      }

      if (reader.ValueTextEquals("Kind"))
      {
        reader.Skip();
        kind = Enum.Parse<ResultKind>(reader.GetString() !);
      }
      else if (reader.ValueTextEquals("Ok"))
      {
        reader.Skip();
        val = JsonSerializer.Deserialize<TOk>(ref reader, options) !;
      }
      else if (reader.ValueTextEquals("Err"))
      {
        reader.Skip();
        err = JsonSerializer.Deserialize<TErr>(ref reader, options) !;
      }

      reader.Read();
    }

    if (kind == ResultKind.Ok)
    {
      if (val is null)
      {
        throw new JsonException("Cannot create an Result.Ok with a null value.");
      }

      return Result<TOk, TErr>.Ok(val);
    }

    if (kind == ResultKind.Err)
    {
      if (err is null)
      {
        throw new JsonException("Cannot create an Result.Err with a null value.");
      }

      return Result<TOk, TErr>.Err(err);
    }

    throw new JsonException($"Could not deserialize a Result with kind '{kind}'.");
  }

  /// <inheritdoc />
  public override void Write(Utf8JsonWriter writer, Result<TOk, TErr> result, JsonSerializerOptions options)
  {
    if (result is null)
    {
      return;
    }

    writer.WriteStartObject();

    writer.WriteString("Kind", result.Kind.ToString());

    if (result.IsOk())
    {
      writer.WritePropertyName("Ok");
      JsonSerializer.Serialize(writer, result.Unwrap(), options);
    }
    else if (result.IsErr())
    {
      writer.WritePropertyName("Err");
      JsonSerializer.Serialize(writer, result.UnwrapErr(), options);
    }

    writer.WriteEndObject();
  }
}