using ResultCs.Tests.Integration.Quadratics;

namespace ResultCs.Tests.Integration;

public class QuadraticsTests
{
  private const string InputPath = "Inputs/";
  private Dictionary<(string, RootType), string> outputAssertions = new ()
  {
    { ("2-real-roots.txt", RootType.Real), "{(3, 0), (-4, 0)}"},
    { ("2-real-roots.txt", RootType.Complex), "{(3+0i, 0), (-4+0i, 0)}"},
  };

  [Theory]
  [InlineData("2-real-roots.txt", RootType.Real)]
  [InlineData("2-real-roots.txt", RootType.Complex)]
  public async Task OutputTests(string inputFile, RootType rootType)
  {
    var expected = outputAssertions[(inputFile, rootType)];
    var actual = await Driver.ReadQuadraticAndOutputAsync(InputPath+inputFile, rootType);

    Assert.Equal(expected, actual);
  }
}