using ResultCs.Tests.Integration.Quadratics;

namespace ResultCs.Tests.Integration;

public class QuadraticsTests
{
  private const string InputPath = "Inputs/";
  private Dictionary<(string, RootType), string> outputAssertions = new ()
  {
    { ("2-real-roots.txt", RootType.Real), "{(3, 0), (-4, 0)}"},
    { ("2-real-roots.txt", RootType.Complex), "{(3+0i, 0), (-4+0i, 0)}"},
    { ("1-real-root.txt", RootType.Real), "{(-5, 0)}"},
    { ("1-real-root.txt", RootType.Complex), "{(-5+0i, 0)}"},
    { ("2-complex-roots.txt", RootType.Real), ((char)157).ToString()},
    { ("2-complex-roots.txt", RootType.Complex), "{(-0.33+1.25i, 0), (-0.33-1.25i, 0)}"},
    { ("1-input.txt", RootType.Real), ErrorStrings.NotEnoughFields },
    { ("1-input.txt", RootType.Complex), ErrorStrings.NotEnoughFields },
    { ("2-inputs.txt", RootType.Real), ErrorStrings.NotEnoughFields },
    { ("2-inputs.txt", RootType.Complex), ErrorStrings.NotEnoughFields },
    { ("empty.txt", RootType.Real), ErrorStrings.NoLines },
    { ("empty.txt", RootType.Complex), ErrorStrings.NoLines },
    { ("2-lines.txt", RootType.Real), ErrorStrings.TooManyLines },
    { ("2-lines.txt", RootType.Complex), ErrorStrings.TooManyLines },
    { ("illegal-input.txt", RootType.Real), string.Format(ErrorStrings.ParsingError, 'b') },
    { ("illegal-input.txt", RootType.Complex), string.Format(ErrorStrings.ParsingError, 'b') },
    { ("missing-file.txt", RootType.Real), string.Format(ErrorStrings.FileNotFound, InputPath+"missing-file.txt") },
    { ("missing-file.txt", RootType.Complex), string.Format(ErrorStrings.FileNotFound, InputPath+"missing-file.txt") },
  };

  [Theory]
  [InlineData("2-real-roots.txt", RootType.Real)]
  [InlineData("2-real-roots.txt", RootType.Complex)]
  [InlineData("1-real-root.txt", RootType.Real)]
  [InlineData("1-real-root.txt", RootType.Complex)]
  [InlineData("2-complex-roots.txt", RootType.Real)]
  [InlineData("2-complex-roots.txt", RootType.Complex)]
  [InlineData("1-input.txt", RootType.Real)]
  [InlineData("1-input.txt", RootType.Complex)]
  [InlineData("2-inputs.txt", RootType.Real)]
  [InlineData("2-inputs.txt", RootType.Complex)]
  [InlineData("empty.txt", RootType.Real)]
  [InlineData("empty.txt", RootType.Complex)]
  [InlineData("2-lines.txt", RootType.Real)]
  [InlineData("2-lines.txt", RootType.Complex)]
  [InlineData("illegal-input.txt", RootType.Real)]
  [InlineData("illegal-input.txt", RootType.Complex)]
  [InlineData("missing-file.txt", RootType.Real)]
  [InlineData("missing-file.txt", RootType.Complex)]
  public async Task OutputTests(string inputFile, RootType rootType)
  {
    var expected = outputAssertions[(inputFile, rootType)];
    var actual = await Driver.ReadQuadraticAndOutputAsync(InputPath+inputFile, rootType);

    Assert.Equal(expected, actual);
  }
}