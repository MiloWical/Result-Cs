using System.Text.Json;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonSerializationTests : IClassFixture<SystemTextJsonSerializationFixture>
{
  readonly SystemTextJsonSerializationFixture fixture;

  public SystemTextJsonSerializationTests(SystemTextJsonSerializationFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Theory]
  [InlineData("test")]
  [InlineData(true)]
  [InlineData(1)]
  [InlineData(1.0f)]
  [InlineData(1.0d)]
  [InlineData('a')]
  [InlineData(null)]
  public void OptionPrimitiveSerializationTests<T>(T value)
  {
    Option<T> option;

    option = value is null ? Option<T>.None() : Option<T>.Some(value);

    var json = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);
    var deserializedOption = JsonSerializer.Deserialize<Option<T>>(json, fixture.JsonSerializerOptions);

    Assert.Equal(option, deserializedOption);
  }

  [Fact]
  public void OptionObjectSerializationTests()
  {
    var student = new Student()
    {
      FirstName = "Milo",
      LastName = "Wical",
      Grade = 100.0,
    };

    var option = Option<Student>.Some(student);

    var json = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);
    var deserializedOption = JsonSerializer.Deserialize<Option<Student>>(json, fixture.JsonSerializerOptions);
    var deserializedStudent = deserializedOption!.Unwrap();

    Assert.Equal(student.FirstName, deserializedStudent.FirstName);
    Assert.Equal(student.LastName, deserializedStudent.LastName);
    Assert.Equal(student.Grade, deserializedStudent.Grade);
  }

}
