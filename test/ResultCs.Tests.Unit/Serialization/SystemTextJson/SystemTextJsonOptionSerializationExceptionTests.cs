using System.Text.Json;
using WicalWare.Components.ResultCs.Serialization.SystemTextJson;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonOptionSerializationExceptionTests : IClassFixture<SerializationTestFixture>
{
  private readonly SerializationTestFixture fixture;

  public SystemTextJsonOptionSerializationExceptionTests(SerializationTestFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Fact]
  public void ExpectingStartObjectTokenTest()
  {
    var json = @"""Kind"":""Some"",""Some"":1}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Option<int>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.ExpectingStartObjectToken, thrownException.Message);
  }

  [Fact]
  public void NullOptionSomeValueTest()
  {
    var json = @"{""Kind"":""Some"",""Some"":null}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Option<int?>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.NullOptionSomeValue, thrownException.Message);
  }

  [Fact]
  public void IllegalKindValueTest()
  {
    var json = @"{""Kind"":""Other"",""Other"":1}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Option<int>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.IllegalOptionKindValue, thrownException.Message);
  }
}
