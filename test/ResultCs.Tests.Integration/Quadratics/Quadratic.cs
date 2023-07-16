using WicalWare.Components.ResultCs;

namespace ResultCs.Tests.Integration.Quadratics;

public class Quadratic
{
  public double QuadraticCoefficient { get; private set; }
  public double LinearCoefficient { get; private set; }
  public double Constant { get; private set; }
  public double Discriminant { get; private set; }

  public Quadratic(double quadraticCoefficient, double linearCoefficient, double constant)
  {
    QuadraticCoefficient = quadraticCoefficient;
    LinearCoefficient = linearCoefficient;
    Constant = constant;

    Discriminant = Math.Pow(LinearCoefficient, 2) - 4 * QuadraticCoefficient * Constant;
  }

  public Option<Root[]> GetRoots(RootType rootType)
  {
    if (rootType == RootType.Real && Discriminant < 0)
    {
      return Option<Root[]>.None();
    }

    Root[] roots;

    if (Discriminant == 0)
    {
      roots = new Root[1];

      roots[0] = new Root(-1 * LinearCoefficient / (2 * QuadraticCoefficient), 0);
    }
    else
    {
      roots = new Root[2];

      var discriminantType = Discriminant < 0 ? RootType.Complex : RootType.Real;

      if(discriminantType == RootType.Real)
      {
        roots[0] = new Root(((-1 * LinearCoefficient) + Math.Sqrt(Discriminant)) / (2 * QuadraticCoefficient), 0);
        roots[1] = new Root(((-1 * LinearCoefficient) - Math.Sqrt(Discriminant)) / (2 * QuadraticCoefficient), 0);
      }

      else
      {
        roots[0] = new Root(-1 * LinearCoefficient / (2 * QuadraticCoefficient), Math.Sqrt(Math.Abs(Discriminant)) / (2 * QuadraticCoefficient));
        roots[0] = new Root(-1 * LinearCoefficient / (2 * QuadraticCoefficient), -1 * Math.Sqrt(Math.Abs(Discriminant)) / (2 * QuadraticCoefficient));
      }
    }

    return Option<Root[]>.Some(roots);
  }

  public Result<string, string> ToString(RootType rootType)
  {
    var roots = GetRoots(rootType);

    if (roots.IsNone())
    {
      return Result<string, string>.Ok(((char)157).ToString());
    }

    var outputStrings = roots.Map(r =>
      {
        var stringList = new List<string>();
        foreach(var x in r)
        {
          var rootString = x.ToString(rootType);

          if (rootString.IsErr())
          {
            return Result<IEnumerable<string>, string>.Err(rootString.UnwrapErr());
          }

          stringList.Add(rootString.Unwrap());
        }

        return Result<IEnumerable<string>, string>.Ok(stringList);
      }
    );

    if(outputStrings.IsNone())
    {
      return Result<string, string>.Err(ErrorStrings.ProcessingError);
    }

    var joinStrings = outputStrings.Unwrap();

    if (joinStrings.IsErr())
    {
      return Result<string, string>.Err(joinStrings.UnwrapErr());
    }

    return Result<string, string>.Ok($"{{{string.Join(", ", joinStrings.Unwrap())}}}");
  }
}
