using System.Text.Json;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonOptionSerializationTests : IClassFixture<SerializationTestFixture>
{
  private readonly SerializationTestFixture fixture;

  public SystemTextJsonOptionSerializationTests(SerializationTestFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.OptionPrimitiveTestCases), MemberType = typeof(SerializationTestFixture))]
  public void OptionPrimitiveSerializationAndDeserializationTests<T>(T value)
  {
    Option<T> option;

    option = value is null ? Option<T>.None() : Option<T>.Some(value);

    var json = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);
    var deserializedOption = JsonSerializer.Deserialize<Option<T>>(json, fixture.JsonSerializerOptions);

    Assert.Equal(option, deserializedOption);
  }

  [Fact]
  public void OptionObjectSerializationAndDeserializationTest()
  {
    var option = Option<Student>.Some(fixture.TestStudent);

    var json = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);
    var deserializedOption = JsonSerializer.Deserialize<Option<Student>>(json, fixture.JsonSerializerOptions);
    var deserializedStudent = deserializedOption!.Unwrap();

    Assert.Equal(fixture.TestStudent.FirstName, deserializedStudent.FirstName);
    Assert.Equal(fixture.TestStudent.LastName, deserializedStudent.LastName);
    Assert.Equal(fixture.TestStudent.Grade, deserializedStudent.Grade);
  }

  [Fact]
  public void OptionCompositeObjectSerializationAndDeserializationTest()
  {
    var option = Option<Class[]>.Some(fixture.TestClassArray);

    var json = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);
    var deserializedOption = JsonSerializer.Deserialize<Option<Class[]>>(json, fixture.JsonSerializerOptions);
    var deserializedClasses = deserializedOption!.Unwrap();

    Assert.NotEmpty(deserializedClasses);
    Assert.Equal(fixture.TestClassArray.Length, deserializedClasses.Length);

    for(var c = 0; c < deserializedClasses.Length; c++)
    {
      Assert.NotEmpty(deserializedClasses[c].Students);
      Assert.Equal(fixture.TestClassArray[c].Students.Length, deserializedClasses[c].Students.Length);

      for(var s = 0; s < deserializedClasses[c].Students.Length; s++)
      {
        Assert.Equal(fixture.TestClassArray[c].Students[s].FirstName, 
          deserializedClasses[c].Students[s].FirstName);

        Assert.Equal(fixture.TestClassArray[c].Students[s].LastName, 
          deserializedClasses[c].Students[s].LastName);

        Assert.Equal(fixture.TestClassArray[c].Students[s].Grade, 
          deserializedClasses[c].Students[s].Grade);
      }
    }
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.OptionJsonSerializedPrimitiveTestCases), MemberType = typeof(SerializationTestFixture))]
  public void OptionPrimitiveSerializationTests<T>(T value, string expectedJson) 
  {
    var option = Option<T>.Some(value);

    var serializedOption = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);

    Assert.Equal(expectedJson, serializedOption);
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.OptionJsonSerializedPrimitiveTestCases), MemberType = typeof(SerializationTestFixture))]
  public void OptionPrimitiveDeserializationTests<T>(T value, string json) 
  {
    var expectedOption = Option<T>.Some(value);

    var deserializedOption = JsonSerializer.Deserialize<Option<T>>(json, fixture.JsonSerializerOptions);

    Assert.Equal(expectedOption, deserializedOption);
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.OptionJsonSerializedStudentTestCases), MemberType = typeof(SerializationTestFixture))]
  public void OptionObjectSerializationTests<T>(string firstName, string lastName, double grade, string expectedJson) 
  {
    var student = new Student()
    {
      FirstName = firstName,
      LastName = lastName,
      Grade = grade,
    };

    var option = Option<Student>.Some(student);

    var serializedOption = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);

    Assert.Equal(expectedJson, serializedOption);
  }

  [Fact]
  public void NullOptionSerializationTest()
  {
    Option<int> option = null !;

    var serializedOption = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);

    Assert.Equal("null", serializedOption);
  }
}
