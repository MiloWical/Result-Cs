namespace ResultCs.Tests.Integration.Quadratics;

public static class ErrorStrings
{
  public const string FileNotFound = "File '{0}' not found.";
  public const string NotEnoughFields = "Not enough fields to create a quadratic.";
  public const string TooManyFields = "Too many fields to create a quadratic.";
  public const string ParsingError = "Could not parse '{0}' to a double.";
  public const string NonZeroImaginaryPart = "Cannot return a real result with a non-zero imaginary part.";
  public const string ProcessingError = "Error processing quadratic roots.";
  public const string NoLines = "Input file contained no lines.";
  public const string TooManyLines = "Input file contained more than 1 line.";
}