using System.Text.Json;
using WicalWare.Components.ResultCs.Serialization.SystemTextJson;

namespace ResultCs.Tests.Unit.Serialization;

public class SerializationTestFixture
{
  private readonly Student testStudent = new Student()
  {
    FirstName = "Milo",
    LastName = "Wical",
    Grade = 100.0,
  };

  private readonly Class[] testClassArray = new[]
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

  public JsonSerializerOptions JsonSerializerOptions { get; init; }

  public Student TestStudent => testStudent;

  public Class[] TestClassArray => testClassArray;

  public SerializationTestFixture()
  {
    JsonSerializerOptions = new JsonSerializerOptions();
    JsonSerializerOptions.Converters.Add(new ResultAndOptionConverterFactory());
  }

  public static IEnumerable<object[]> OptionPrimitiveTestCases()
  {
    yield return new object[] { "test" };
    yield return new object[] { true };
    yield return new object[] { 1 };
    yield return new object[] { 1.0f };
    yield return new object[] { 1.0d };
    yield return new object[] { 'a' };
    yield return new object[] { null! };
  }

  public static IEnumerable<object[]> ResultPrimitiveTestCases()
  {
    yield return new object[] { "test", null! };
    yield return new object[] { true, null! };
    yield return new object[] { 1, null! };
    yield return new object[] { 1.0f, null! };
    yield return new object[] { 1.0d, null! };
    yield return new object[] { 'a', null! };
    yield return new object[] { null!, "error" };
    yield return new object[] { null!, -1 };
  }
  public static IEnumerable<object[]> OptionJsonSerializedPrimitiveTestCases()
  {
    yield return new object[] { 1, @"{""Kind"":""Some"",""Some"":1}" };
    yield return new object[] { "test", @"{""Kind"":""Some"",""Some"":""test""}" };
  }

  public static IEnumerable<object[]> ResultJsonSerializedPrimitiveTestCases()
  {
    yield return new object[] { 1, null!, @"{""Kind"":""Ok"",""Ok"":1}" };
    yield return new object[] { "test", null!, @"{""Kind"":""Ok"",""Ok"":""test""}" };
    yield return new object[] { null!, -1, @"{""Kind"":""Err"",""Err"":-1}" };
    yield return new object[] { null!, "error", @"{""Kind"":""Err"",""Err"":""error""}" };
  }

  public static IEnumerable<object[]> OptionJsonSerializedStudentTestCases()
  {
    yield return new object[] { "Milo", "Wical", 100.0, @"{""Kind"":""Some"",""Some"":{""FirstName"":""Milo"",""LastName"":""Wical"",""Grade"":100}}" };
    yield return new object[] { "Anakin", "Skywalker", 0.0, @"{""Kind"":""Some"",""Some"":{""FirstName"":""Anakin"",""LastName"":""Skywalker"",""Grade"":0}}" };
  }

  public static IEnumerable<object[]> ResultJsonSerializedStudentTestCases()
  {
    yield return new object[] { "Milo", "Wical", 100.0, @"{""Kind"":""Ok"",""Ok"":{""FirstName"":""Milo"",""LastName"":""Wical"",""Grade"":100}}" };
    yield return new object[] { "Anakin", "Skywalker", 0.0, @"{""Kind"":""Ok"",""Ok"":{""FirstName"":""Anakin"",""LastName"":""Skywalker"",""Grade"":0}}" };
  }
}