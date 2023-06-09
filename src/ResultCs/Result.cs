// Copyright (c) Milo Wical. All rights reserved.

using System.Diagnostics.CodeAnalysis;

namespace WicalWare.Components.ResultCs;

// https://doc.rust-lang.org/std/result/enum.Result.html

/// <summary>
/// A wrapper that helps prevent passing <c>null</c> values within code.
///
/// It can some in 2 different flavors.
///
/// <ul>
/// <li>Ok(TOk), which contains a wrapped value of type <c>TSome</c>.
/// This value can be extracted from the <c>Option</c> using the <c>Unwrap</c>
/// functions.</li>
/// <li>Err(TErr), which contains a wrapped error of the type <c>TErr</c>.
/// This value can be extracted from the <c>Result</c> using the <c>UnwrapErr</c>
/// functions.</li>
/// </ul>
/// </summary>
/// <typeparam name="TOk">The type of the <c>Ok</c> value for this <c>Result</c>.</typeparam>
/// <typeparam name="TErr">The type of the <c>Err</c> value for this <c>Result</c>.</typeparam>
public class Result<TOk, TErr>
{
  // Comment Imp Test SignatureResult{U, E}
  // ✓       ✓   ✓    Result{U, E} And{U}(Result{U, E} optB)
  // ✓       ✓   ✓    Result{U, E} AndThen{U}(Func{T, Result{U, E}} f)
  // ✓       ✓   ✓    Option{E} Err()
  // ✓       ✓   ✓    T Expect(string msg)
  // ✓       ✓   ✓    E ExpectErr(string msg)
  // ✓       ✓   ✓    bool IsErr()
  // ✓       ✓   ✓    bool IsOk()
  // ✓       ✓   ✓    IEnumerable{Option{T}} Iter()
  // ✓       ✓   ✓    Result{U, E} Map{U}(Func{T, U} op)
  // ✓       ✓   ✓    Result{T, F} MapErr{F}(Func{E, F} op)
  // ✓       ✓   ✓    U MapOr{U}(U def, Func{T, U} f)
  // ✓       ✓   ✓    U MapOrElse{U}(Func{E, U} def, Func{T, U} f)
  // ✓       ✓   ✓    Option{T} Ok()
  // ✓       ✓   ✓    Result{T, F} Or{F}(Result{T, F} res)
  // ✓       ✓   ✓    Result{T, F} OrElse{F}(Func{E, Result{T, F}} op)
  // ✓       ✓   ✓    Option{Result{U, E}} Transpose{U}()
  // ✓       ✓   ✓    T Unwrap()
  // ✓       ✓   ✓    E UnwrapErr()
  // ✓       ✓   ✓    T UnwrapOr(T def)
  // ✓       ✓   ✓    T UnwrapOrDefault()
  // ✓       ✓   ✓    T UnwrapOrElse(Func{E, T} op)
  //
  // Experimental signatures (not implemented)
  //                  bool Contains{U}(U x)
  //                  bool ContainsErr{F}(F f)
  //                  Result{T, E} Flatten()
  //                  Result{T, E} Inspect(Action{T} f)
  //                  Result{T, E} InspectErr(Action{E} f)
  //                  E IntoErr()
  //                  T IntoOk()
  //                  bool IsErrAnd(Func{E, bool} f)
  //                  bool IsOkAnd(Func{T, bool} f)

  // If this Result is Ok, the value wrapped by the response.
  private TOk? val;

  // If this result is Err, the error wrapped by the response.
  private TErr? err;

  /// <summary>
  /// Gets the <see cref="ResultKind"/> of the current <c>Result</c>.
  /// </summary>
  public ResultKind Kind { get; private set; }

  /// <summary>
  /// Convenience override of the <c>==</c> operator.
  /// </summary>
  /// <param name="res1">The first <c>Result</c> to compare.</param>
  /// <param name="res2">The second <c>Result</c> to compare.</param>
  /// <returns>The result of <c>res1.Equals(res2)</c>.</returns>
  public static bool operator ==(Result<TOk, TErr> res1, Result<TOk, TErr> res2)
  {
    if (res1 is null)
    {
      if (res2 is null)
      {
        return true;
      }

      return false;
    }

    return res1.Equals(res2);
  }

  /// <summary>
  /// Convenience override of the <c>!=</c> operator.
  /// </summary>
  /// <param name="res1">The first <c>Result</c> to compare.</param>
  /// <param name="res2">The second <c>Result</c> to compare.</param>
  /// <returns>The result of <c>!res1.Equals(res2)</c>.</returns>
  public static bool operator !=(Result<TOk, TErr> res1, Result<TOk, TErr> res2) => !(res1 == res2);

  /// <summary>
  /// Generates a <c>Result</c> with a <c>ResultKind</c> of <see cref="ResultKind.Ok"/>.
  ///
  /// The value passed is not permitted to be <c>null</c>.
  /// </summary>
  /// <param name="value">The value to wrap in the <c>Result</c>.</param>
  /// <returns>The <c>Result</c> of kind <c>Ok</c> with the provided value wrapped.</returns>
  public static Result<TOk, TErr> Ok(TOk value)
  {
    ArgumentNullException.ThrowIfNull(value);

    var result = new Result<TOk, TErr>();
    result.val = value;
    result.Kind = ResultKind.Ok;

    return result;
  }

  /// <summary>
  /// Generates a <c>Result</c> with a <c>ResultKind</c> of <see cref="ResultKind.Err"/>.
  ///
  /// The error passed is not permitted to be <c>null</c>.
  /// </summary>
  /// <param name="err">The error to wrap in the <c>Result</c>.</param>
  /// <returns>The <c>Result</c> of kind <c>Err</c> with the provided error wrapped.</returns>
  public static Result<TOk, TErr> Err(TErr err)
  {
    ArgumentNullException.ThrowIfNull(err);

    var result = new Result<TOk, TErr>();
    result.err = err;
    result.Kind = ResultKind.Err;

    return result;
  }

  /// <summary>
  /// Tests two <c>Result</c> objects for equality.
  ///
  /// Overrides <see cref="object.Equals(object?)"/>.
  /// </summary>
  /// <param name="other">The <c>Result</c> to test <c>this</c> against.</param>
  /// <returns><c>true</c> if one of the following is true:
  /// <ul>
  /// <li>The two <c>Result</c> objects are <c>Ok</c> and their wrapped values are the same.</li>
  /// <li>The two <c>Result</c> objects are <c>Err</c>.</li>
  /// </ul>
  /// <c>false</c> otherwise.</returns>
  public override bool Equals(object? other)
  {
    if (other is null)
    {
      return false;
    }

    if (other is not Result<TOk, TErr>)
    {
      return false;
    }

    var otherResult = (Result<TOk, TErr>)other;

    if (this.Kind != otherResult.Kind)
    {
      return false;
    }

    if (this.IsErr() && otherResult.IsErr())
    {
      return true;
    }

    if (this.val!.Equals(otherResult.Unwrap()))
    {
      return true;
    }

    return false;
  }

  /// <summary>
  /// Override of <see cref="object.GetHashCode()"/>.
  /// </summary>
  /// <returns><c>base.GetHashCode()</c>.</returns>
  public override int GetHashCode() => base.GetHashCode();

  /// <summary>
  /// Asserts whether a result is <c>Ok</c> and returns values accordingly.
  ///
  /// Arguments passed to <c>And</c> are eagerly evaluated; if you are passing the>
  /// result of a function call, it is recommended to use
  /// <see cref="AndThen"/>, which is lazily evaluated.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(2);
  /// var y = Result{string, string}.Err("late error");
  /// Assert.Equal(x.And{string}(y), Result{string, string}.Err("late error"));
  ///
  /// var x = Result{int, string}.Err("early error");
  /// var y = Result{string, string}.Ok("foo");
  /// Assert.Equal(x.And{string}(y), Result{string, string}.Err("early error"));
  ///
  /// var x = Result{int, string}.Err("not a 2");
  /// var y = Result{string, string}.Err("late error");
  /// Assert.Equal(x.And{string}(y), Result{string, string}.Err("not a 2"));
  ///
  /// var x = Result{int, string}.Ok(2);
  /// var y = Result{string, string}.Ok("different result type");
  /// Assert.Equal(x.And{string}(y), Result{string, string}.Ok("different result type"));
  /// </code>
  /// </summary>
  /// <param name="res">The option to return if the current option is <c>Ok</c>.</param>
  /// <typeparam name="TOkOut">The underlying type of <c>res</c>.</typeparam>
  /// <returns>Returns <c>res</c> if the result is <c>Ok</c>,
  /// otherwise returns the <c>Err</c> value of <c>self</c>.</returns>
  public Result<TOkOut, TErr> And<TOkOut>(Result<TOkOut, TErr> res)
  {
    if (this.Kind == ResultKind.Err)
    {
      return Result<TOkOut, TErr>.Err(this.err!);
    }

    if (res.Kind == ResultKind.Err)
    {
      return Result<TOkOut, TErr>.Err(res.UnwrapErr());
    }

    return res;
  }

  /// <summary>
  /// Calls <c>op</c> if the result is <c>Ok</c>,
  /// otherwise returns the <c>Err</c> value of <c>self</c>.
  ///
  /// This function can be used for control flow based on <c>Result</c> values.
  ///
  /// <example>
  /// Examples
  ///
  /// <code>
  /// public Result{string, string} SquareThenToString(int x)
  /// {
  ///   try
  ///   {
  ///     int product;
  ///
  ///     checked
  ///     {
  ///       product = x*x;
  ///     }
  ///
  ///     return Result{string, string}.Ok(product.ToString());
  ///   }
  ///   catch(OverflowException oe)
  ///   {
  ///     return Result{string, string}.Err("overflowed");
  ///   }
  /// }
  ///
  /// Assert.Equal(Result{int, string}.Ok(2).AndThen(SquareThenToString), Result{string, string}.Ok(4.ToString()));
  /// Assert.Equal(Result{int, string}.Ok(int.MaxValue).AndThen(SquareThenToString), Result{string, string}.Err("overflowed"));
  /// Assert.Equal(Result{int, string}.Err("not a number").AndThen(SquareThenToString), Result{string, string}.Err("not a number"));
  /// </code>
  ///
  /// Often used to chain fallible operations that may return <c>Err</c>.
  ///
  /// <code>
  /// using System.IO;
  ///
  /// &#8725;&#8725;Note: on Windows "/" maps to "C:\"
  /// var rootModifiedTime = Result{DirectoryInfo, string}.Ok(new DirectoryInfo("/")).AndThen(di =} Result{DateTime, string}.Ok(di.LastWriteTime));
  /// Assert.True(rootModifiedTime.IsOk());
  ///
  /// var shouldFail = Result{DirectoryInfo, string}.Ok(new DirectoryInfo("/bad/path")).AndThen(di =} Result{DateTime, string}.Ok(di.LastWriteTime));
  /// Assert.True(shouldFail.IsErr());
  /// Assert.TypeOf{Exception}(shouldFail.UnwrapErr());
  /// </code>
  /// </example>
  /// </summary>
  /// <param name="f">The function to call to evaluate the current <c>Result</c>.</param>
  /// <typeparam name="TOut">The underlying type of the result of calling <c>f</c>.</typeparam>
  /// <returns>The result of calling <c>f</c>,
  /// otherwise returns the <c>Err</c> value of <c>self</c>.</returns>
  public Result<TOut, TErr> AndThen<TOut>(Func<TOk, Result<TOut, TErr>> f)
  {
    if (this.IsErr())
    {
      return Result<TOut, TErr>.Err(this.UnwrapErr());
    }

    return f.Invoke(this.Unwrap());
  }

  /// <summary>
  /// Converts from <c>Result{T, E}</c> to <c>Option{E}</c>.
  ///
  /// Converts <c>self</c> into an <c>Option{E}</c>, consuming <c>self</c>,
  /// and discarding the success value, if any.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(2);
  /// Assert.Equal(x.Err(), Option{string}.None());
  ///
  /// var x = Result{int, string}.Err("Nothing here");
  /// Assert.Equal(x.Err(), Option{string}.Some("Nothing here"));
  /// </code>
  /// </summary>
  /// <returns>The <c>Err</c> value of <c>self</c>, wrapped in
  /// and <c>Option{E}</c>.</returns>
  public Option<TErr> Err()
  {
    if (this.IsErr())
    {
      return Option<TErr>.Some(this.err!);
    }

    return Option<TErr>.None();
  }

  /// <summary>
  /// Obtains the underlying value of <c>Ok</c>, throwing <c>Err</c> as
  /// an exception (panicking) if <c>self</c> is not <c>Ok</c>.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the <c>Err</c>
  /// case explicitly, or call <c><see cref="UnwrapOr"/></c>,
  /// <c><see cref="UnwrapOrElse"/></c>, or <c><see cref="UnwrapOrDefault"/></c>
  ///
  /// Panics if the value is an <c>Err</c>, with a panic message including the
  /// passed message, and the content of the <c>Err</c>.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Err("emergency failure");
  /// Assert.Throws{PanicException}(() => x.Expect("Testing expect")); // panics with <c>Testing expect: emergency failure</c>
  /// </code>
  ///
  /// Recommended Message Style
  ///
  /// We recommend that <c>expect</c> messages are used to describe the reason you
  /// <i>expect</i> the <c>Result</c> should be <c>Ok</c>.
  ///
  /// <code>
  /// var path = Environment.GetEnvironmentVariable("IMPORTANT_PATH")
  ///     .Expect("env variable <c>IMPORTANT_PATH</c> should be set by <c>wrapper_script.sh</c>");
  /// </code>
  ///
  /// <b>>Hint</b>: If you're having trouble remembering how to phrase expect
  /// error messages remember to focus on the word "should" as in "env
  /// variable should be set by blah" or "the given binary should be available
  /// and executable by the current user".
  ///
  /// For more detail on expect message styles and the reasoning behind our recommendation please
  /// refer to the section on <see href="https://doc.rust-lang.org/std/error/index.html#common-message-styles">Common Message
  /// Styles</see>.
  /// </summary>
  /// <param name="msg">The message to panic with.</param>
  /// <returns>The unwrapped value of <c>self</c>.</returns>
  [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "StyleCop doesn't respect the null-forgiving operator (!)")]
  public TOk Expect(string msg)
  {
    if (this.IsOk())
    {
      return this.Unwrap();
    }

    if (this.err is Exception)
    {
      throw new PanicException(msg, (this.err as Exception)!);
    }

    throw new PanicException($"{msg}: {this.UnwrapErr()!.ToString()}");
  }

  /// <summary>
  /// Returns the contained <c>Err</c> value, consuming the <c>self</c> value.
  ///
  /// Panics if the value is an <c>Ok</c>, with a panic message including the
  /// passed message, and the content of the <c>Ok</c>.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(10);
  /// x.ExpectErr("Testing ExpectErr"); // panics with <c>Testing ExpectErr: 10</c>
  /// </code>
  /// </summary>
  /// <param name="msg">The message to panic with.</param>
  /// <returns>The unwrapped value of <c>self</c>.</returns>
  [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "StyleCop doesn't respect the null-forgiving operator (!)")]
  public TErr ExpectErr(string msg)
  {
    if (this.IsErr())
    {
      return this.UnwrapErr();
    }

    throw new PanicException($"{msg}: {this.Unwrap()!.ToString()}");
  }

  /// <summary>
  /// Returns <c>true</c> if the result is <c>Err</c>.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(-3);
  /// Assert.False(x.IsErr());
  ///
  /// var x = Result{int, string}.Err("Some error message");
  /// Assert.True(x.IsErr());
  /// </code>
  /// </summary>
  /// <returns><c>true</c> if the result is <c>Err</c>.</returns>
  public bool IsErr()
  {
    return this.Kind == ResultKind.Err;
  }

  /// <summary>
  /// Returns <c>true</c> if the result is <c>Ok</c>.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(-3);
  /// Assert.True(x.IsOk());
  ///
  /// var x = Result{int, string}.Err("Some error message");
  /// Assert.False(x.IsOk());
  /// </code>
  /// </summary>
  /// <returns><c>true</c> if the result is <c>Ok</c>.</returns>
  public bool IsOk()
  {
    return this.Kind == ResultKind.Ok;
  }

  /// <summary>
  /// Returns an enumerable over the possibly contained value.
  ///
  /// The iterator yields one value if the result is <see cref="ResultKind.Ok"/>, otherwise none.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(7);
  /// var iter = x.Iter().GetEnumerator();
  ///
  /// Assert.True(iter.MoveNext());
  /// Assert.Equal(Option{int}.Some(7), iter.Current);
  /// Assert.False(iter.MoveNext());
  ///
  /// var x = Result{int, string}.Err("nothing!");
  /// var iter = x.Iter().GetEnumerator();
  ///
  /// Assert.True(iter.MoveNext());
  /// Assert.Equal(Option{int}.None(), iter.Current);
  /// Assert.False(iter.MoveNext());
  /// </code>
  /// </summary>
  /// <returns>An <c>IEnumerable{Option{T}}</c> containing a single value.
  /// <ul>
  /// <li>If <c>this</c> is <c>ResultKind.Ok</c>, the value is an <c>OptionKind.Some</c> with the unwrapped value of <c>this</c></li>
  /// <li>If <c>this</c> is <c>ResultKind.Ok</c>, the value is an <c>OptionKind.None</c></li>
  /// </ul></returns>
  public IEnumerable<Option<TOk>> Iter()
  {
    if (this.IsOk())
    {
      return new List<Option<TOk>> { Option<TOk>.Some(this.Unwrap()) };
    }

    return new List<Option<TOk>> { Option<TOk>.None() };
  }

  /// <summary>
  /// Maps a <c>Result{T, E}</c> to <c>Result{U, E}</c> by applying a function to a
  /// contained <c>Ok</c> value, leaving an <c>Err</c> value untouched.
  ///
  /// This function can be used to compose the results of two functions.
  ///
  /// Examples
  ///
  /// Print the numbers on each line of a string multiplied by two.
  ///
  /// <code>
  /// var lines = "1\n2\n3\n4\nbad_num";
  ///
  /// foreach(var line in lines.Split('\n'))
  /// {
  ///   var result = int.TryParse(line, out var num) ? Result{int, string}.Ok(num)  = Result{int, string}.Err($"Could not parse: {line}");
  ///
  ///   var mapped = result.Map(i = i * 2);
  ///
  ///   switch (mapped.Kind)
  ///   {
  ///    case ResultKind.Ok:
  ///       Assert.Equal(num * 2, mapped.Unwrap());
  ///       break;
  ///     case ResultKind.Err:
  ///       Assert.Contains(line, mapped.UnwrapErr());
  ///       break;
  ///   };
  /// }
  /// </code>
  /// </summary>
  /// <param name="op">The function to be applied to the value if <c>this</c> is <c>ResultKind.Ok</c>.</param>
  /// <typeparam name="TOut">The output type of <c>op</c>.</typeparam>
  /// <returns>A <c>Result</c> that's the output of applying <c>op</c> to the current <c>Result</c>'s <c>Ok</c> value.</returns>
  public Result<TOut, TErr> Map<TOut>(Func<TOk, TOut> op)
  {
    if (this.IsErr())
    {
      return Result<TOut, TErr>.Err(this.UnwrapErr());
    }

    return Result<TOut, TErr>.Ok(op.Invoke(this.Unwrap()));
  }

  /// <summary>
  /// Maps a <c>Result{T, E}</c> to <c>Result{T, F}</c> by applying a function to a
  /// contained <c>Err</c> value, leaving an <c>Ok</c> value untouched.
  ///
  /// This function can be used to pass through a successful result while handling
  /// an error.
  ///
  ///
  /// Basic usage:
  ///
  /// <code>
  /// public string stringify(int x)
  /// {
  ///   return $"error code: {x}";
  /// }
  ///
  /// var x = Result{int, int}.Ok(2);
  /// Assert.Equal(x.MapErr(stringify), Result{int, string}.Ok(2));
  ///
  /// var x = Result{int, int}.Err(13);
  /// Assert.Equal(x.MapErr(stringify), Result{int, string}.Err("error code: 13"));
  /// </code>
  /// </summary>
  /// <param name="op">The function to be applied to the value if <c>this</c> is <c>ResultKind.Err</c>.</param>
  /// <typeparam name="TOut">The output type of <c>op</c>.</typeparam>
  /// <returns>A <c>Result</c> that's the output of applying <c>op</c> to the current <c>Result</c>'s <c>Err</c> value.</returns>
  public Result<TOk, TOut> MapErr<TOut>(Func<TErr, TOut> op)
  {
    if (this.IsOk())
    {
      return Result<TOk, TOut>.Ok(this.Unwrap());
    }

    return Result<TOk, TOut>.Err(op.Invoke(this.UnwrapErr()));
  }

  /// <summary>
  /// Returns the provided default (if <c>Err</c>), or
  /// applies a function to the contained value (if <c>Ok</c>),
  ///
  /// Arguments passed to <c>MapOr</c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>MapOrElse</c>,
  /// which is lazily evaluated.
  ///
  /// <code>
  /// var x = Result{string, string}.Ok("foo");
  /// Assert.Equal(3, x.MapOr(42, v =} v.Length));
  ///
  /// var x = Result{string, string}.Err("bar");
  /// Assert.Equal(42, x.MapOr(42, v =} v.Length));
  /// </code>
  /// </summary>
  /// <param name="def">The default value to return if <c>this</c> is <c>ResultKind.Err</c>.</param>
  /// <param name="f">The function to be applied to the value if <c>this</c> is <c>ResultKind.Ok</c>.</param>
  /// <typeparam name="TOut">The output type of <c>f</c>.</typeparam>
  /// <returns>If <c>this</c> is <c>ResultKind.Ok</c>, a <c>Result</c> that's the output of applying
  /// <c>f</c> to the current <c>Result</c>'s <c>Ok</c> value; otherwise <c>def</c>.</returns>
  public TOut MapOr<TOut>(TOut def, Func<TOk, TOut> f)
  {
    if (this.IsErr())
    {
      return def;
    }

    return f.Invoke(this.Unwrap());
  }

  /// <summary>
  /// Maps a <c>Result{T, E}</c> to <c>U</c> by applying fallback function <c>default</c> to
  /// a contained <c>Err</c> value, or function <c>f</c> to a contained <c>Ok</c> value.
  ///
  /// This function can be used to unpack a successful result
  /// while handling an error.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var k = 21;
  ///
  /// var x = Result{string, string}.Ok("foo");
  /// Assert.Equal(x.MapOrElse(_ => k * 2, v => v.Length), 3);
  ///
  /// var x = Result{string, string}.Err("bar");
  /// Assert.Equal(x.MapOrElse(_ => k * 2, v => v.Length), 42);
  /// </code>
  /// </summary>
  /// <param name="def">The default function to invoke on the <c>Err</c> value
  /// if <c>this</c> is <c>ResultKind.Err</c>.</param>
  /// <param name="f">The function to be applied to the value if <c>this</c> is <c>ResultKind.Ok</c>.</param>
  /// <typeparam name="TOut">The output type of <c>def</c> and <c>f</c>.</typeparam>
  /// <returns>If <c>this</c> is <c>ResultKind.Ok</c>, a value that's the output of applying
  /// <c>f</c> to the current <c>Result</c>'s <c>Ok</c> value; otherwise a value that's the
  /// output of applying <c>def</c> to the current <c>Result</c>'s <c>Err</c> value.</returns>
  public TOut MapOrElse<TOut>(Func<TErr, TOut> def, Func<TOk, TOut> f)
  {
    if (this.IsErr())
    {
      return def.Invoke(this.UnwrapErr());
    }

    return f.Invoke(this.Unwrap());
  }

  /// <summary>
  /// Converts from <c>Result{T, E}</c> to <c>Option{T}</c>.
  ///
  /// Converts <c>self</c> into an <c>Option{T}</c>, consuming <c>self</c>,
  /// and discarding the error, if any.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(2);
  /// Assert.Equal(x.Ok(), Option{int}.Some(2));
  ///
  /// var x = Result{int, string}.Err("Nothing here");
  /// Assert.Equal(x.Ok(), Option{int}.None());
  /// </code>
  /// </summary>
  /// <returns>An <c>Option</c> that contains a <c>Some</c> value if
  /// <c>this</c> is <c>ResultKind.Ok</c>; <c>None</c> otherwise.</returns>
  public Option<TOk> Ok()
  {
    if (this.IsErr())
    {
      return Option<TOk>.None();
    }

    return Option<TOk>.Some(this.val!);
  }

  /// <summary>
  /// Returns <c>res</c> if the result is <c>Err</c>, otherwise returns the <c>Ok</c> value of <c>self</c>.
  ///
  /// Arguments passed to <c>Or()</c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use <see cref="OrElse"/>, which is
  /// lazily evaluated.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(2);
  /// var y = Result{int, string}.Err("late error");
  /// Assert.Equal(x.Or(y), Result{int, string}.Ok(2));
  ///
  /// var x = Result{int, string}.Err("early error");
  /// var y = Result{int, string}.Ok(2);
  /// Assert.Equal(x.Or(y), Result{int, string}.Ok(2));
  ///
  /// var x = Result{int, string}.Err("not a 2");
  /// var y = Result{int, string}.Err("late error");
  /// Assert.Equal(x.Or(y), Result{int, string}.Err("late error"));
  ///
  /// var x = Result{int, string}.Ok(2);
  /// var y = Result{int, string}.Ok(100);
  /// Assert.Equal(x.Or(y), Result{int, string}.Ok(2));
  /// </code>
  /// </summary>
  /// <param name="res">The <c>Result</c> to return if <c>this</c> is <c>Err</c>.</param>
  /// <typeparam name="TErrOut">The <c>Err</c> type of the return <c>Result</c>.</typeparam>
  /// <returns><c>res</c> if the result is <c>Err</c>, otherwise returns the
  /// <c>Ok</c> value of <c>self</c>.</returns>
  public Result<TOk, TErrOut> Or<TErrOut>(Result<TOk, TErrOut> res)
  {
    if (this.IsErr())
    {
      return res;
    }

    return Result<TOk, TErrOut>.Ok(this.Unwrap());
  }

  /// <summary>
  /// Calls <c>op</c> if the result is <c>Err</c>, otherwise returns the <c>Ok</c> value of <c>self</c>.
  ///
  /// This function can be used for control flow based on result values.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// public Result{int, int} sq(int x)
  /// {
  ///   return Result{int, int}.Ok(x * x);
  /// }
  ///
  /// public Result{int, int} err(int x)
  /// {
  ///   return Result{int, int}.Err(x);
  /// }
  ///
  /// Assert.Equal(Result{int, int}.Ok(2).OrElse(sq).OrElse(sq), Result{int, int}.Ok(2));
  /// Assert.Equal(Result{int, int}.Ok(2).OrElse(err).OrElse(sq), Result{int, int}.Ok(2));
  /// Assert.Equal(Result{int, int}.Err(3).OrElse(sq).OrElse(err), Result{int, int}.Ok(9));
  /// Assert.Equal(Result{int, int}.Err(3).OrElse(err).OrElse(err), Result{int, int}.Err(3));
  /// </code>
  /// </summary>
  /// <param name="op">The function to invoke when <c>this</c> is <c>Err</c>.</param>
  /// <typeparam name="TErrOut">The <c>Err</c> return type of <c>op</c>.</typeparam>
  /// <returns>The <c>op</c> result if <c>this</c> is <c>Err</c>; otherwise
  /// the <c>Ok</c> value of <c>self</c>.</returns>
  public Result<TOk, TErrOut> OrElse<TErrOut>(Func<TErr, Result<TOk, TErrOut>> op)
  {
    if (this.IsOk())
    {
      return Result<TOk, TErrOut>.Ok(this.Unwrap());
    }

    return op.Invoke(this.UnwrapErr());
  }

  /// <summary>
  /// Transposes a <c>Result</c> of an <c>Option</c> into an <c>Option</c> of a <c>Result</c>.
  ///
  /// <c>Ok(None)</c> will be mapped to <c>None</c>.
  /// <c>Ok(Some)</c> and <c>Err</c> will be mapped to <c>Some(Ok)</c> and <c>Some(Err)</c>.
  ///
  /// <code>
  ///
  /// var x = Result{Option{int}, string}.Ok(Option{int}.Some(5));
  /// var y = Option{Result{int, string}}.Some(Result{int, string}.Ok(5));
  /// Assert.Equal(x.Transpose(), y);
  ///
  /// </code>
  /// </summary>
  /// <typeparam name="TOkOut">The internal type of the <c>Result{Option{}}</c>. (Note: this is a
  /// deviation from the Rust signature because C# doesn't implement enum values the way
  /// Rust does.)</typeparam>
  /// <returns>A <c>Result</c> of an <c>Option</c> into an <c>Option</c> of a <c>Result</c>.</returns>
  public Option<Result<TOkOut, TErr>> Transpose<TOkOut>()
  {
    if (!typeof(TOk).IsGenericType || typeof(TOk).GetGenericTypeDefinition() != typeof(Option<>))
    {
      throw new PanicException($"The wrapped type is {this.val!.GetType()}; expecting Option<{typeof(TOkOut)}>");
    }

    if (this.IsOk())
    {
      var optionType = typeof(TOk).GetGenericArguments()[0];

      if (typeof(TOkOut) != optionType)
      {
        throw new PanicException($"The wrapped value is {optionType} not assignable to type parameter {typeof(TOkOut)}");
      }

      Option<TOkOut>? option = this.Unwrap() as Option<TOkOut>;

      if (option!.IsNone())
      {
        return Option<Result<TOkOut, TErr>>.None();
      }

      return Option<Result<TOkOut, TErr>>.Some(Result<TOkOut, TErr>.Ok(option.Unwrap()));
    }

    return Option<Result<TOkOut, TErr>>.Some(Result<TOkOut, TErr>.Err(this.UnwrapErr()));
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value, consuming the <c>self</c> value.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the <c>Err</c>
  /// case explicitly, or call <c>UnwrapOr</c>, <c>UnwrapOrElse</c>, or
  /// <c>UnwrapOrDefault</c>.
  ///
  /// # Panics
  ///
  /// Panics if the value is an <c>Err</c>, with a panic message provided by the
  /// <c>Err</c>'s value.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(2);
  /// Assert.Equal(x.Unwrap(), 2);
  /// </code>
  ///
  /// <code>
  /// var x = Result{int, string}.Err("emergency failure");
  /// x.Unwrap(); // panics with <c>emergency failure</c>
  /// </code>
  /// </summary>
  /// <returns>The wrapped <c>Ok</c> value.</returns>
  [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "StyleCop doesn't respect the null-forgiving operator (!)")]
  public TOk Unwrap()
  {
    if (this.IsErr())
    {
      throw new PanicException(this.UnwrapErr()!.ToString()!);
    }

    return this.val!;
  }

  /// <summary>
  /// Returns the contained <c>Err</c> value, consuming the <c>self</c> value.
  ///
  /// # Panics
  ///
  /// Panics if the value is an <c>Ok</c>, with a custom panic message provided
  /// by the <c>Ok</c>'s value.
  ///
  /// <code>
  /// var x = Result{int, string}.Ok(2);
  /// x.UnwrapErr(); // panics with <c>2</c>
  /// </code>
  ///
  /// <code>
  /// var x = Result{int, string}.Err("emergency failure");
  /// Assert.Equal(x.UnwrapErr(), "emergency failure");
  /// </code>
  /// </summary>
  /// <returns>The wrapped <c>Err</c> value.</returns>
  [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "StyleCop doesn't respect the null-forgiving operator (!)")]
  public TErr UnwrapErr()
  {
    if (this.IsOk())
    {
      throw new PanicException(this.Unwrap()!.ToString()!);
    }

    return this.err!;
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value or a provided default.
  ///
  /// Arguments passed to <c>UnwrapOr</c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>UnwrapOrElse</c>,
  /// which is lazily evaluated.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var def = 2;
  /// var x = Result{int, string}.Ok(9);
  /// Assert.Equal(9, x.UnwrapOr(def));
  ///
  /// var x = Result{int, string}.Err("error");
  /// Assert.Equal(def, x.UnwrapOr(def));
  /// </code>
  /// </summary>
  /// <param name="def">The default value to return if <c>this</c> is <c>Err</c>.</param>
  /// <returns>The <c>Ok</c> value if <c>this</c> is <c>Ok</c>; <c>def</c> otherwise.</returns>
  public TOk UnwrapOr(TOk def)
  {
    if (this.IsErr())
    {
      return def;
    }

    return this.val!;
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value or a default
  ///
  /// Consumes the <c>self</c> argument then, if <c>Ok</c>, returns the contained
  /// value, otherwise if <c>Err</c>, returns the default value for that
  /// type.
  ///
  /// Examples
  ///
  /// Converts a string to an integer, turning poorly-formed strings
  /// into 0 (the default value for integers). <c>Parse</c> converts
  /// a string to an <c>int</c>, returning an <c>Err</c> on error.
  ///
  /// <code>
  /// var goodYearFromInput = "1909";
  /// var badYearFromInput = "190blarg";
  /// var goodYear = Parse(goodYearFromInput).UnwrapOrDefault();
  /// var badYear = Parse(badYearFromInput).UnwrapOrDefault();
  ///
  /// Assert.Equal(1909, goodYear);
  /// Assert.Equal(0, badYear);
  /// </code>
  /// </summary>
  /// <returns>The <c>Ok</c> value if <c>this</c> is <c>Ok</c>; <c>default(T)</c> otherwise.</returns>
  public TOk? UnwrapOrDefault()
  {
    if (this.IsErr())
    {
      return default(TOk);
    }

    return this.val;
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value or computes it from a closure.
  ///
  /// Basic usage:
  ///
  /// <code>
  /// public int Count(string x)
  /// {
  ///   return x.Length;
  /// }
  ///
  /// Assert.Equal(2, Result{int, string}.Ok(2).UnwrapOrElse(Count));
  /// Assert.Equal(3, Result{int, string}.Err("foo").UnwrapOrElse(Count));
  /// </code>
  /// </summary>
  /// <param name="op">The function to be applied to the value if <c>this</c> is <c>ResultKind.Err</c>.</param>
  /// <returns>The <c>Ok</c> value if <c>this</c> is <c>Ok</c>; otherwise the result of passing the
  /// <c>Err</c> value to <c>op</c>.</returns>
  public TOk UnwrapOrElse(Func<TErr, TOk> op)
  {
    if (this.IsOk())
    {
      return this.val!;
    }

    return op.Invoke(this.err!);
  }
}