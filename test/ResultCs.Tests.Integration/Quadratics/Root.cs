using WicalWare.Components.ResultCs;

namespace ResultCs.Tests.Integration.Quadratics;

public class Root
{
  public double RealPart { get;  private set; }
  public double ImaginaryPart { get; private set; }

  public Root(double realPart, double imaginaryPart)
  {
    RealPart = realPart;
    ImaginaryPart = imaginaryPart;
  }

  public override bool Equals(object? obj)
  {
    if (obj == null || ! obj.GetType().Equals(typeof(Root)))
    {
      return false;
    }

    var root = (Root)obj;

    return RealPart == root.RealPart && ImaginaryPart == root.ImaginaryPart;
  }

  public override int GetHashCode()
  {
    return base.GetHashCode();
  }

  public Result<string, string> ToString(RootType rootType)
  {
    if(rootType == RootType.Complex)
      return Result<string, string>.Ok(
        $"({RealPart:0.##}{(ImaginaryPart >= 0 ? "+" : string.Empty)}{ImaginaryPart:0.##}i, 0)");

    if (ImaginaryPart != 0)
      return Result<string, string>.Err(ErrorStrings.NonZeroImaginaryPart);

    return Result<string, string>.Ok($"({RealPart.ToString("0.##")}, 0)");
  }
}