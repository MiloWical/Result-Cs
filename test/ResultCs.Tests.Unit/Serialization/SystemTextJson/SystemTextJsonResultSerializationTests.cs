using System.Text.Json;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonResultSerializationTests : IClassFixture<SerializationTestFixture>
{
  private readonly SerializationTestFixture fixture;

  public SystemTextJsonResultSerializationTests(SerializationTestFixture systemTextJsonSerializationFixture)
  {
    fixture = systemTextJsonSerializationFixture;
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.ResultPrimitiveTestCases), MemberType = typeof(SerializationTestFixture))]
  public void ResultPrimitiveSerializationAndDeserializationTests<T, E>(T value, E err)
  {
    Result<T,E> result;

    result = value is null ? Result<T, E>.Err(err) : Result<T, E>.Ok(value);

    var json = JsonSerializer.Serialize(result, fixture.JsonSerializerOptions);
    var deserializedResult = JsonSerializer.Deserialize<Result<T, E>>(json, fixture.JsonSerializerOptions);

    Assert.Equal(result, deserializedResult);
  }

  [Fact]
  public void ResultObjectSerializationAndDeserializationTest()
  {
    var result = Result<Student, string>.Ok(fixture.TestStudent);

    var json = JsonSerializer.Serialize(result, fixture.JsonSerializerOptions);
    var deserializedResult = JsonSerializer.Deserialize<Result<Student, string>>(json, fixture.JsonSerializerOptions);
    var deserializedStudent = deserializedResult!.Unwrap();

    Assert.Equal(fixture.TestStudent.FirstName, deserializedStudent.FirstName);
    Assert.Equal(fixture.TestStudent.LastName, deserializedStudent.LastName);
    Assert.Equal(fixture.TestStudent.Grade, deserializedStudent.Grade);
  }

  [Fact]
  public void ResultCompositeObjectSerializationAndDeserializationTest()
  {
    var Result = Result<Class[], string>.Ok(fixture.TestClassArray);

    var json = JsonSerializer.Serialize(Result, fixture.JsonSerializerOptions);
    var deserializedResult = JsonSerializer.Deserialize<Result<Class[], string>>(json, fixture.JsonSerializerOptions);
    var deserializedClasses = deserializedResult!.Unwrap();

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
  [MemberData(nameof(SerializationTestFixture.ResultJsonSerializedPrimitiveTestCases), MemberType = typeof(SerializationTestFixture))]
  public void ResultPrimitiveSerializationTests<T, E>(T value, E error, string expectedJson) 
  {
    var result = value is null ? Result<T, E>.Err(error) : Result<T, E>.Ok(value);

    var serializedResult = JsonSerializer.Serialize(result, fixture.JsonSerializerOptions);

    Assert.Equal(expectedJson, serializedResult);
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.ResultJsonSerializedPrimitiveTestCases), MemberType = typeof(SerializationTestFixture))]
  public void ResultPrimitiveDeserializationTests<T, E>(T value, E error, string json) 
  {
    var expectedResult = value is null ? Result<T, E>.Err(error) : Result<T, E>.Ok(value);

    var deserializedResult = JsonSerializer.Deserialize<Result<T, E>>(json, fixture.JsonSerializerOptions);

    Assert.Equal(expectedResult, deserializedResult);
  }

  [Theory]
  [MemberData(nameof(SerializationTestFixture.ResultJsonSerializedStudentTestCases), MemberType = typeof(SerializationTestFixture))]
  public void ResultObjectSerializationTests<T, _>(string firstName, string lastName, double grade, string expectedJson) 
  {
    var student = new Student()
    {
      FirstName = firstName,
      LastName = lastName,
      Grade = grade,
    };

    var Result = Result<Student, _>.Ok(student);

    var serializedResult = JsonSerializer.Serialize(Result, fixture.JsonSerializerOptions);

    Assert.Equal(expectedJson, serializedResult);
  }

  [Fact]
  public void NullResultSerializationTest()
  {
    Result<int, string> result = null !;

    var serializedResult = JsonSerializer.Serialize(result, fixture.JsonSerializerOptions);

    Assert.Equal("null", serializedResult);
  }
}
