using WicalWare.Components.ResultCs.Serialization.SystemTextJson;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonConverterFactoryTests : IClassFixture<SerializationTestFixture>
{
  private readonly SerializationTestFixture fixture;

  public SystemTextJsonConverterFactoryTests(SerializationTestFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Fact]
  public void ResultAndOptionConverterFactoryIllegalTypeTest()
  {
    var converterFactory = new ResultAndOptionConverterFactory();
    
    Assert.Throws<ArgumentException>(() => converterFactory.CreateConverter(typeof(IEnumerable<string>), fixture.JsonSerializerOptions));
  }
}
