# Result C#

## Additional Information

If you'd like to explore the package more and see in-depth details about this package, please go here: https://milowical.github.io/Result-Cs/

## Motivation

`null` is a problematic value for many developers, and in the Rust language a conscious decision to NOT include `null` was made to prevent a whole class of problems. As stated in [The Rust Programming Language](https://doc.rust-lang.org/book/ch06-01-defining-an-enum.html?highlight=null#the-option-enum-and-its-advantages-over-null-values):

> Programming language design is often thought of in terms of which features you include, but the features you exclude are important too. Rust doesn’t have the null feature that many other languages have. Null is a value that means there is no value there. In languages with null, variables can always be in one of two states: null or not-null.
> 
> In his 2009 presentation “Null References: The Billion Dollar Mistake,” Tony Hoare, the inventor of null, has this to say:
>
  > "I call it my billion-dollar mistake. At that time, I was designing the first comprehensive type system for references in an object-oriented language. My goal was to ensure that all use of references should be absolutely safe, with checking performed automatically by the compiler. But I couldn’t resist the temptation to put in a null reference, simply because it was so easy to implement. This has led to innumerable errors, vulnerabilities, and system crashes, which have probably caused a billion dollars of pain and damage in the last forty years."

If you'd like to hear it in his own words: [Null References: The Billion Dollar Mistake](https://www.youtube.com/watch?v=YYkOWzrO3xg)
  <a href="">
    <img src="./images/billion-dollar-mistake.png" alt="">
  </a>
</p>

The amount of code that I've written over my career led me to appreciate the insightfulness of the Rust language developers in avoiding `null`, so I decided to write this package in order to give .NET developers the same opportunity for goodness. :grin: :zap: :metal:

## Overview

The Rust paradigm for avoiding `null` as a concept involves the use of using enumerations ("enums") that wrap values against which typechecking can be performed.

### `Option<T>`

The simplest value implemented in this library is the `Option<T>` type. It consists of 2 enum values:

- `None`: Indicates that the `Option` has no usable value (closest to the intuitive notion of `null`)

- `Some`: Indicates that the `Option` has a value of type `T` that can be "unwrapped" (Rust idiom) to get to the internal value, which will never be `null`.

#### `Option` Example

Below is an example of using `Option<T>` to introduce `null`-safety:

```c#
string text = "Bacon ipsum";

var nullSafeLength = text.NullSafeLength();

if (nullSafeLength.IsNone())
{
  Console.WriteLine("The length for the string doesn't exist.");
}
else if (nullSafeLength.IsSome())
{
  Console.WriteLine($"The length of the string is: {nullSafeLength.Unwrap()}");
}

public static class NullSafeExtensions
{
  public static Option<int> NullSafeLength(this string s)
  {
    if (s is null)
    {
      return Option<int>.None();
    }

    return Option<int>.Some(s.Length);
  }
}
```

### `Result<T, E>`

The `Result<T, E>` type is slightly more complex than the `Option<T>` type, though the general thought processes are directly applicable. The `Result` can wrap values of one of two types:

- `T`: The wrapped type to return in the event the resulting value has semantic value.

- `E`: An error type that allows the implementer to return a value that indicates that the operation was unsuccessful _without_ either:
  
  - Returning `null`
  - Throwing an exception

The `Result` comes in two flavors:

- `Ok`: Indicates that the output has validity for additional processing and can be safely unwrapped.

- `Err`: Indicates that the result of the operation was unsuccessful and carries additional details important to the caller about the failure.

#### `Result` Example

A basic application that uses the `Result<T, E>` type might look like this:

```c#
Console.Write("Enter a numerator: ");

var numeratorStr = Console.ReadLine();

Console.Write("Enter a denominator: ");

var denominatorStr = Console.ReadLine();

var result = ToDecimal(numeratorStr, denominatorStr);

if (result.IsErr())
{
  Console.WriteLine(result.UnwrapErr());
}
else if (result.IsOk())
{
  Console.WriteLine($"The resulting decimal is: {result.Unwrap()}");
}

public Result<double, string> ToDecimal(string numStr, string denStr)
{
  if (!double.TryParse(numStr, out var num))
  {
    return Result<double, string>.Err($"The value '{numStr}' could not be parsed to a double.");
  }

  if (!double.TryParse(denStr, out var den))
  {
    return Result<double, string>.Err($"The value '{denStr}' could not be parsed to a double.");
  }

  if (denStr == 0)
  {
    return Result<double, string>.Err("Cannot divide by zero.");
  }

  return Result<double, string>.Ok(num / den);
}
```