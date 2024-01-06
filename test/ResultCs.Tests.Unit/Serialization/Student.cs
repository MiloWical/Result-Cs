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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  public Student()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

  {

  }

  public Student(string firstName, string lastName, double grade)
  {
    FirstName = firstName;
    LastName = lastName;
    Grade = grade;
  }
}
