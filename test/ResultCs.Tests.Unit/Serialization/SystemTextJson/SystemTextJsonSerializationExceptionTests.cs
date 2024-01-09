using System.Text.Json;
using WicalWare.Components.ResultCs.Serialization.SystemTextJson;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonSerializationExceptionTests : IClassFixture<SystemTextJsonSerializationFixture>
{
  private readonly SystemTextJsonSerializationFixture fixture;

  public SystemTextJsonSerializationExceptionTests(SystemTextJsonSerializationFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Fact]
  public void ExpectingStartObjectTokenTest()
  {
    var json = @"""Kind"":""Some"",""Some"":1}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Option<int>>(json, fixture.JsonSerializerOptions));

    Assert.Equal(DeserializationExceptionMessages.ExpectingStartObjectToken, thrownException.Message);
  }
}
