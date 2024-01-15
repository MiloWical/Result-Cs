using System.Text.Json;
using WicalWare.Components.ResultCs.Serialization.SystemTextJson;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonResultSerializationExceptionTests : IClassFixture<SerializationTestFixture>
{
  private readonly SerializationTestFixture fixture;

  public SystemTextJsonResultSerializationExceptionTests(SerializationTestFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Fact]
  public void ExpectingStartObjectTokenTest()
  {
    var json = @"""Kind"":""Ok"",""Ok"":1}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Result<int, string>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.ExpectingStartObjectToken, thrownException.Message);
  }

  [Fact]
  public void NullResultOkValueTest()
  {
    var json = @"{""Kind"":""Ok"",""Ok"":null}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Result<int?, string>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.NullResultOkValue, thrownException.Message);
  }

  [Fact]
  public void NullResultErrValueTest()
  {
    var json = @"{""Kind"":""Err"",""Err"":null}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Result<int?, string>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.NullResultErrValue, thrownException.Message);
  }

  [Fact]
  public void IllegalKindValueTest()
  {
    var json = @"{""Kind"":""Other"",""Other"":1}";

    var thrownException = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Result<int, string>>(json, fixture.JsonSerializerOptions));

    Assert.Contains(DeserializationExceptionMessages.IllegalResultKindValue, thrownException.Message);
  }
}
