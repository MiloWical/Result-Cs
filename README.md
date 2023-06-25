# Result C#

## Motivation

`null` is a problematic value for many developers, and in the Rust language a conscious decision to NOT include `null` was made to prevent a whole class of problems. As stated in [The Rust Programming Language](https://doc.rust-lang.org/book/ch06-01-defining-an-enum.html?highlight=null#the-option-enum-and-its-advantages-over-null-values):

> Programming language design is often thought of in terms of which features you include, but the features you exclude are important too. Rust doesn’t have the null feature that many other languages have. Null is a value that means there is no value there. In languages with null, variables can always be in one of two states: null or not-null.
> 
> In his 2009 presentation “Null References: The Billion Dollar Mistake,” Tony Hoare, the inventor of null, has this to say:
>
  > "I call it my billion-dollar mistake. At that time, I was designing the first comprehensive type system for references in an object-oriented language. My goal was to ensure that all use of references should be absolutely safe, with checking performed automatically by the compiler. But I couldn’t resist the temptation to put in a null reference, simply because it was so easy to implement. This has led to innumerable errors, vulnerabilities, and system crashes, which have probably caused a billion dollars of pain and damage in the last forty years."

If you'd like to hear it in his own words:

[![Null References: The Billion Dollar Mistake](./.readme/billion-dollar-mistake.png)](https://www.youtube.com/watch?v=YYkOWzrO3xg)

The amount of code that I've written over my career led me to appreciate the insightfulness of the Rust language developers in avoiding `null`, so I decided to write this package in order to give .NET developers the same opportunity for goodness. :grin: :zap: :metal:

## Overview

The Rust paradigm for avoiding `null` as a concept involves the use of using enumerations ("enums") that wrap values against which typechecking can be performed.

### Enumerations

One of the distinctions between Rust and C# is that C# doesn't have the notion of enums that can optionally carry values. The implementation of this pattern still uses the concept of enums for defining the context that underlying value represents, but has to implement it in the manner the C# language dictates.

### Memory Management

Another distinction is that C# uses a garbage collector for memory management, so understanding the concepts of ownership and mutability are largely transparent to the C# developer, while Rust developers are acutely (and at times painfully) aware of them. Consequently, many of the Rust idioms for managing ownership and consumption of values don't apply so the corresponding interfaces in the Rust API have been left out to simplify the C# API surface area.

### Experimental Functions

Additionally, there are many experimental API signatures that exist as optional in the Rust API and must be compiled in optionally when building a Rust solution. For now, the C# implementation has ignored the experimental signatures. It's possible that I might implement compiler flags for the experimental API functions, but it depends on the value gained by users. If implemented, that will probably be in a future minor release.

## Enum Types

It should be noted that for all of the enum types implemented in this library, _**`null` parameters and return values are expressly DISALLOWED**_ and will result in a `PanicException` to be thrown. (More about [Panics](#panics) later.)

Because these types implement enums as a "kind" of response, you can use pattern matching syntax on the enum values to help process the `Option` or `Result`; this is consistent with the Rust idioms for evaluating enums with wrapped values.

### `Option<T>`

The simplest value implemented in this library is the `Option<T>` type. It consists of 2 enum values:

- `None`: Indicates that the `Option` has no usable value (closest to the intuitive notion of `null`)

- `Some`: Indicates that the `Option` has a value that can be "unwrapped" (Rust idiom) to get to the internal value, which will never be `null`.

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

The `Result<T, E>` type is slightly more complex than the `Option<T>` type, though the general thought processes are directly applicable. The `Result` can have one of two types:

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

## Panics

The Rust idiom for dealing with exceptional cases is similar to the golang mechanism - panics. In many cases, it's superior (IMHO) than using exceptions to control execution flow, particularly when you're dealing with library code.

To maintain a reasonable level of parity with the Rust mechanics of dealing with error conditions, this library contains a relatively generic [`PanicException`](./src/ResultCs/PanicException.cs) that attempts to provide a similar experience that in Rust. You'll see that the exception is intended to be used within the context of the library, hence [no public constructors](./src/ResultCs/PanicException.cs#L14-L47). It's expected that developers handle panics in the context of the `Option` and `Result` values and convert that into the local semantics for the consuming application.

## Parting Words

The entire notion of being able to program away a class of problems that has caused so much havoc in the computer science and software engineering worlds is alluring to mme to say the least. I feel like those of us in the trenches have a great opportunity to further our craft and make the (software) world a better, safer place. 

Keep calm, code on, and rock out. :metal: