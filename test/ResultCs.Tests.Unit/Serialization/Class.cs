namespace ResultCs.Tests.Unit.Serialization;

public class Class
{
  public Student[] Students { get; init; }

  public Class()
  {
    Students = new Student[0];
  }

  public Class(Student[] students)
  {
    Students = students;
  }

}
