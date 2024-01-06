using System.Text.Json;
using WicalWare.Components.ResultCs.Serialization.SystemTextJson;

namespace ResultCs.Tests.Unit.Serialization;

public class SystemTextJsonSerializationFixture
{
  public JsonSerializerOptions JsonSerializerOptions { get; init; }

  public SystemTextJsonSerializationFixture()
  {
    JsonSerializerOptions = new JsonSerializerOptions();
    JsonSerializerOptions.Converters.Add(new ResultAndOptionConverterFactory());
  }
}