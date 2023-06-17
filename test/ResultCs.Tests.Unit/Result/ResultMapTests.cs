namespace ResultCs.Tests.Unit.Result;

public class ResultMapTests
{
  [Fact]
  public void ResultMapTest()
  {
    var lines = "1\n2\n3\n4\nbad_num";

    foreach (var line in lines.Split('\n'))
    {
      var result = int.TryParse(line, out var num) ? Result<int, string>.Ok(num) : Result<int, string>.Err($"Could not parse: {line}");

      var mapped = result.Map(i => i * 2);

      switch (mapped.Kind)
      {
        case ResultKind.Ok:
          Assert.Equal(num * 2, mapped.Unwrap());
          break;
        case ResultKind.Err:
          Assert.Contains(line, mapped.UnwrapErr());
          break;
      }
    }
  }

  [Fact]
  public void ResultMapNullDelegateTest()
  {
    Assert.Throws<PanicException>(() => Result<int, int>.Ok(1).Map<int>(null!));
  }

  [Fact]
  public void ResultMapDelegateReturnsNullTest()
  {
    Assert.Throws<PanicException>(() => Result<int, int>.Ok(1).Map<int?>(_ => null!));
  }
}