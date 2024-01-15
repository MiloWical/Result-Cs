// Copyright (c) Milo Wical. All rights reserved.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace WicalWare.Components.ResultCs.Serialization.SystemTextJson;

/// <summary>
/// A factory that scaffolds the <see cref="OptionConverter{TSome}"/> and
/// <see cref="ResultConverter{TOk, TErr}"/>.
/// </summary>
public class ResultAndOptionConverterFactory : JsonConverterFactory
{
  /// <inheritdoc />
  public override bool CanConvert(Type typeToConvert)
  {
    if (!typeToConvert.IsGenericType)
    {
      return false;
    }

    var genericType = typeToConvert.GetGenericTypeDefinition();

    return genericType == typeof(Result<,>) || genericType == typeof(Option<>);
  }

  /// <inheritdoc />
  public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
  {
    var baseType = typeToConvert.GetGenericTypeDefinition();

    Type converterType;

    if (baseType == typeof(Result<,>))
    {
      converterType = typeof(ResultConverter<,>);
    }
    else if (baseType == typeof(Option<>))
    {
      converterType = typeof(OptionConverter<>);
    }
    else
    {
      throw new ArgumentException($"Could not generate Option or Result converter for generic type {baseType} of type {typeToConvert}.", nameof(typeToConvert));
    }

    var genericTypes = typeToConvert.GenericTypeArguments;

    var converterInstanceType = converterType.MakeGenericType(genericTypes);
    return Activator.CreateInstance(converterInstanceType) as JsonConverter;
  }
}