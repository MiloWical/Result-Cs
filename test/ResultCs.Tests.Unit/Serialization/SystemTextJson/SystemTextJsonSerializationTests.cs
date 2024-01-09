using System.Text.Json;

namespace ResultCs.Tests.Unit.Serialization.SystemTextJson;

public class SystemTextJsonSerializationTests : IClassFixture<SystemTextJsonSerializationFixture>
{
  private readonly SystemTextJsonSerializationFixture fixture;

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

  [Fact]
  public void OptionCompositeObjectSerializationAndDeserializationTest()
  {
    var classes = new[]
    {
      new Class
      {
        Students = new []
        {
          new Student("A", "A", 1.0),
          new Student("B", "B", 2.0),
          new Student("C", "C", 3.0),
        },
      },
      new Class
      {
        Students = new []
        {
          new Student("X", "X", 4.0),
          new Student("Y", "Y", 5.0),
          new Student("Z", "Z", 6.0),
        },
      },
    };

    var option = Option<Class[]>.Some(classes);

    var json = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);
    var deserializedOption = JsonSerializer.Deserialize<Option<Class[]>>(json, fixture.JsonSerializerOptions);
    var deserializedClasses = deserializedOption!.Unwrap();

    Assert.NotEmpty(deserializedClasses);
    Assert.Equal(classes.Length, deserializedClasses.Length);

    for(var c = 0; c < deserializedClasses.Length; c++)
    {
      Assert.NotEmpty(deserializedClasses[c].Students);
      Assert.Equal(classes[c].Students.Length, deserializedClasses[c].Students.Length);

      for(var s = 0; s < deserializedClasses[c].Students.Length; s++)
      {
        Assert.Equal(classes[c].Students[s].FirstName, 
          deserializedClasses[c].Students[s].FirstName);

        Assert.Equal(classes[c].Students[s].LastName, 
          deserializedClasses[c].Students[s].LastName);

        Assert.Equal(classes[c].Students[s].Grade, 
          deserializedClasses[c].Students[s].Grade);
      }
    }
  }

  [Theory]
  [InlineData(1, @"{""Kind"":""Some"",""Some"":1}")]
  [InlineData("test", @"{""Kind"":""Some"",""Some"":""test""}")]
  public void OptionPrimitiveSerializationTests<T>(T value, string expectedJson) 
  {
    var option = Option<T>.Some(value);

    var serializedOption = JsonSerializer.Serialize(option, fixture.JsonSerializerOptions);

    Assert.Equal(expectedJson, serializedOption);
  }

  [Theory]
  [InlineData(@"{""Kind"":""Some"",""Some"":1}", 1)]
  [InlineData(@"{""Kind"":""Some"",""Some"":""test""}", "test")]
  public void OptionPrimitiveDeserializationTests<T>(string json, T value) 
  {
    var expectedOption = Option<T>.Some(value);

    var deserializedOption = JsonSerializer.Deserialize<Option<T>>(json, fixture.JsonSerializerOptions);

    Assert.Equal(expectedOption, deserializedOption);
  }

  [Theory]
  [InlineData("Milo", "Wical", 100.0, @"{""Kind"":""Some"",""Some"":{""FirstName"":""Milo"",""LastName"":""Wical"",""Grade"":100}}")]
  [InlineData("Anakin", "Skywalker", 0.0, @"{""Kind"":""Some"",""Some"":{""FirstName"":""Anakin"",""LastName"":""Skywalker"",""Grade"":0}}")]
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
