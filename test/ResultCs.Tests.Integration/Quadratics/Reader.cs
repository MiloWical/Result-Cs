using WicalWare.Components.ResultCs;

namespace ResultCs.Tests.Integration.Quadratics;

public class Reader
{
  public static async Task<Result<Quadratic, string>> ReadInputFileAsync(string file)
  {
    if(!File.Exists(file))
    {
      return Result<Quadratic, string>.Err(string.Format(ErrorStrings.FileNotFound, file));
    }

    var lines = await File.ReadAllLinesAsync(file);

    if (lines.Length == 0)
    {
      return Result<Quadratic, string>.Err(ErrorStrings.NoLines);
    }

    if (lines.Length > 1)
    {
      return Result<Quadratic, string>.Err(ErrorStrings.TooManyLines);
    }

    var fields = lines[0].Trim().Split(' ');

    if (fields.Length < 3)
    {
      return Result<Quadratic, string>.Err(ErrorStrings.NotEnoughFields);
    }

    if (fields.Length > 3)
    {
      return Result<Quadratic, string>.Err(ErrorStrings.TooManyFields);
    }

    double[] quadraticFields = new double[3];

    for (var i = 0; i < fields.Length; i++)
    {
      if (!double.TryParse(fields[i], out var quadraticField))
      {
        return Result<Quadratic, string>.Err(string.Format(ErrorStrings.ParsingError, fields[i]));
      }

      quadraticFields[i] = quadraticField;
    }

    return Result<Quadratic, string>.Ok(
      new Quadratic(quadraticFields[0], quadraticFields[1], quadraticFields[2])
    );
  }
}