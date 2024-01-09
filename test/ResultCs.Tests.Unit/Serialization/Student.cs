using System.ComponentModel;

namespace ResultCs.Tests.Unit.Serialization;

/// <summary>
/// Test class for serialization
/// </summary>
public class Student
{
  public string FirstName { get; init; }
  public string LastName { get; init; }
  public double Grade { get; init; }

  public Student()
  {
    FirstName = string.Empty;
    LastName = string.Empty;
    Grade = -1.0;
  }

  public Student(string firstName, string lastName, double grade)
  {
    FirstName = firstName;
    LastName = lastName;
    Grade = grade;
  }
}
