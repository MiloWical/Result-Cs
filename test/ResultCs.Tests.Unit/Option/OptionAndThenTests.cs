namespace ResultCs.Tests.Unit.Option;

public class OptionAndThenTests
{
  [Fact]
  public void OptionAndThenSomeTest()
  {
    Assert.Equal(Option<int>.Some(2).AndThen(SqThenToString), Option<string>.Some(4.ToString()));
    Assert.Equal(Option<int>.Some(1_000_000).AndThen(SqThenToString), Option<string>.None());
  }

  [Fact]
  public void OptionAndThenNoneTest()
  {
    Assert.Equal(Option<int>.None().AndThen(SqThenToString), Option<string>.None());
  }

  [Fact]
  public void OptionAndThenSomeDelegateTest()
  {
    var arr_2D = new string[2][];
    arr_2D[0] = new string[] { "A0", "A1" };
    arr_2D[1] = new string[] { "B0", "B1" };

    var item_0_1 = GetRow(arr_2D, 0).AndThen(row => GetElement(row, 1));
    Assert.Equal(Option<string>.Some("A1"), item_0_1);
  }

  [Fact]
  public void OptionAndThenNoneDelegateTest()
  {
    var arr_2D = new string[2][];
    arr_2D[0] = new string[] { "A0", "A1" };
    arr_2D[1] = new string[] { "B0", "B1" };

    var item_2_0 = GetRow(arr_2D, 2).AndThen(row => GetElement(row, 0));
    Assert.Equal(Option<string>.None(), item_2_0);
  }

  private Option<string> SqThenToString(int x)
  {
    try
    {
      checked
      {
        return Option<string>.Some((x * x).ToString());
      }
    }
    catch (OverflowException)
    {
      return Option<string>.None();
    }
  }

  private Option<string[]> GetRow(string[][] arr, int rowNum)
  {
    if(rowNum >= 0 && rowNum < arr.Length)
      return Option<string[]>.Some(arr[rowNum]);

    return Option<string[]>.None();
  }

  private Option<string> GetElement(string[] arr, int colNum)
  {
    if(colNum >= 0 && colNum < arr.Length)
      return Option<string>.Some(arr[colNum]);

    return Option<string>.None();
  }
}