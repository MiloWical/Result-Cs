// Copyright (c) Milo Wical. All rights reserved.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace WicalWare.Components.ResultCs.Serialization.SystemTextJson;

/// <summary>
/// A converter for System.Text.Json that handles serializing
/// Option{TSome} values.
/// </summary>
/// <typeparam name="TSome">The underlying Some type of the Option.</typeparam>
public class OptionConverter<TSome> : JsonConverter<Option<TSome>>
{
  /// <inheritdoc />
  public override Option<TSome>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    OptionKind kind = (OptionKind)(-1);
    TSome? val = default;

    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException(DeserializationExceptionMessages.ExpectingStartObjectToken);
    }

    reader.Read();

    while (reader.TokenType != JsonTokenType.EndObject)
    {
      if (reader.ValueTextEquals("Kind"))
      {
        reader.Skip();
        try
        {
          kind = Enum.Parse<OptionKind>(reader.GetString() !);
        }
        catch (ArgumentException)
        {
          throw new JsonException($"{DeserializationExceptionMessages.IllegalKindValue} ('{reader.GetString() !}')");
        }
      }
      else if (reader.ValueTextEquals("Some"))
      {
        val = JsonSerializer.Deserialize<TSome?>(ref reader, options) !;
      }

      reader.Read();
    }

    if (kind == OptionKind.None)
    {
      return Option<TSome>.None();
    }

    if (val is null)
    {
      throw new JsonException(DeserializationExceptionMessages.NullOptionSomeValue);
    }

    return Option<TSome>.Some(val);
  }

  /// <inheritdoc />
  public override void Write(Utf8JsonWriter writer, Option<TSome> option, JsonSerializerOptions options)
  {
    writer.WriteStartObject();

    writer.WriteString("Kind", option.Kind.ToString());

    if (option.IsSome())
    {
      writer.WritePropertyName("Some");
      JsonSerializer.Serialize(writer, option.Unwrap(), options);
    }

    writer.WriteEndObject();
  }
}
