namespace ResultCs.Tests.Integration.Quadratics;

public class Driver
{
  public async static Task<string> ReadQuadraticAndOutputAsync(string file, RootType rootType)
  {
    var quadratic = await Reader.ReadInputFileAsync(file);

    if(quadratic.IsErr())
    {
      return quadratic.UnwrapErr();
    }

    var output = quadratic.Unwrap().ToString(rootType);

    if (output.IsErr())
    {
      return output.UnwrapErr();
    }

    return output.Unwrap();
  }
}