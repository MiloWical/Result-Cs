namespace WicalWare.Components.ResultCs;

// TODO: Reread the comments and correct the formatting to C# styling!
// TODO: Check the <return> tags on all comments!

// https://doc.rust-lang.org/std/result/enum.Result.html
public class Result<T, E>
{
  // Comment Imp Test Signature
  // ✓       ✓        Result<U, E> And<U>(Result<U, E> optB)
  //                  Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  //                  Option<E> Err()
  //                  T Expect(string msg)
  //                  E ExpectErr(string msg)
  //                  bool IsErr()
  //                  bool IsOk()
  //                  IEnumerable<Option<T>> Iter()
  //                  Result<U, E> Map<U>(Func<T, U> op)
  //                  Result<T, F> MapErr<F>(Func<E, F> op)
  //                  U MapOr<U>(U def, Func<T, U> f)
  //                  U MapOrElse<U>(Func<E, U> def, Func<T, U> f)
  //                  Option<T> Ok()
  //                  Result<T, F> Or<F>(Result<T, F> res)
  //                  Result<T, F> OrElse<F>(Func<E, Result<T, F>> op) 
  //                  Option<Result<T, E>> Transpose()
  //                  T Unwrap()
  //                  E UnwrapErr()
  //                  T UnwrapOr(T def)
  //                  T UnwrapOrDefault()
  //                  T UnwrapOrElse(Func<E, T> op)
  //
  // Experimental signatures (not implemented)
  //                  bool Contains<U>(U x)
  //                  bool ContainsErr<F>(F f)
  //                  Result<T, E> Flatten()
  //                  Result<T, E> Inspect(Action<T> f)
  //                  Result<T, E> InspectErr(Action<E> f)
  //                  E IntoErr()
  //                  T IntoOk()
  //                  bool IsErrAnd(Func<E, bool> f)
  //                  bool IsOkAnd(Func<T, bool> f)

  public ResultKind Kind { get; private set; }
  private T? _value;
  private E? _err;

  public static Result<T, E> Ok(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    var result = new Result<T, E>();
    result._value = value;
    result.Kind = ResultKind.Ok;

    return result;
  }

  public static Result<T, E> Err(E err)
  {
    ArgumentNullException.ThrowIfNull(err);

    var result = new Result<T, E>();
    result._err = err;
    result.Kind = ResultKind.Err;

    return result;
  }

  public override bool Equals(object? other)
  {
    if(other == null) ArgumentNullException.ThrowIfNull(other);

    var otherResult = (Result<T, E>) other;

    if(this.Kind != otherResult.Kind)
      return false;

    if(this.IsErr() && otherResult.IsErr())
      return true;

    if(_value == null)
    {
      if(otherResult.Unwrap() == null)
        return true;

      return false;
    }

    if(_value.Equals(otherResult.Unwrap()))
      return true;

    return false;
  }

  public static bool operator == (Result<T, E> res1, Result<T, E> res2) => res1.Equals(res2);

  public static bool operator != (Result<T, E> res1, Result<T, E> res2) => !(res1.Equals(res2));

  public override int GetHashCode() => ((object) this).GetHashCode(); 

  /// <summary>
  /// Asserts whether a result is <c>Ok</c> and returns values accordingly.
  /// 
  /// Arguments passed to <c>And</c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use 
  /// <see cref="AndThen"><c>AndThen</c></see>, which is lazily evaluated.
  ///
  /// <example>
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Result<int, string>.Ok(2);
  /// var y = Result<string, string>.Err("late error");
  /// Assert.Equal(x.And<string>(y), Result<string, string>.Err("late error"));
  ///
  /// var x = Result<int, string>.Err("early error");
  /// var y = Result<string, string>.Ok("foo");
  /// Assert.Equal(x.And<string>(y), Result<string, string>.Err("early error"));
  ///
  /// var x = Result<int, string>.Err("not a 2");
  /// var y = Result<string, string>.Err("late error");
  /// Assert.Equal(x.And<string>(y), Result<string, string>.Err("not a 2"));
  ///
  /// var x = Result<int, string>.Ok(2);
  /// var y = Result<string, string>.Ok("different result type");
  /// Assert.Equal(x.And<string>(y), Result<string, string>.Ok("different result type"));
  /// </code>
  /// </summary>
  /// <param name="res">The option to return if the current option is <c>Ok</c></param>
  /// <typeparam name="U">The underlying type of <c>res</c></typeparam>
  /// <returns>Returns <c>res</c> if the result is <c>Ok</c>, 
  /// otherwise returns the <c>Err</c> value of <c>self</c>.</returns>
  public Result<U, E> And<U>(Result<U, E> res)
  {
    if(Kind == ResultKind.Err)
      return Result<U, E>.Err(_err!);

    if(res.Kind == ResultKind.Err)
      return res.Err();

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
  /// fn sq_then_to_string(x: u32) -> Result<String, &'static str> {
  ///     x.checked_mul(x).map(|sq| sq.to_string()).ok_or("overflowed")
  /// }
  ///
  /// assert_eq!(Ok(2).and_then(sq_then_to_string), Ok(4.to_string()));
  /// assert_eq!(Ok(1_000_000).and_then(sq_then_to_string), Err("overflowed"));
  /// assert_eq!(Err("not a number").and_then(sq_then_to_string), Err("not a number"));
  /// <code>
  ///
  /// Often used to chain fallible operations that may return <c>Err</c>.
  ///
  /// <code>
  /// use std::{io::ErrorKind, path::Path};
  ///
  /// // Note: on Windows "/" maps to "C:\"
  /// let root_modified_time = Path::new("/").metadata().and_then(|md| md.modified());
  /// assert!(root_modified_time.is_ok());
  ///
  /// let should_fail = Path::new("/bad/path").metadata().and_then(|md| md.modified());
  /// assert!(should_fail.is_err());
  /// assert_eq!(should_fail.unwrap_err().kind(), ErrorKind::NotFound);
  /// </code>
  /// </example>
  /// </summary>
  /// <param name="f">The function to call to evaluate the current <c>Result</c></param>
  /// <typeparam name="U">The underlying type of the result of calling <c>f</c></typeparam>
  /// <returns>The result of calling <c>f</c>, 
  /// otherwise returns the <c>Err</c> value of <c>self</c>.</returns>
  public Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Converts from <c>Result<T, E></c> to <c>Option<E></c>.
  ///
  /// Converts <c>self</c> into an <c>Option<E></c>, consuming <c>self</c>,
  /// and discarding the success value, if any.
  ///
  /// <example>
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.err(), None);
  ///
  /// let x: Result<u32, &str> = Err("Nothing here");
  /// assert_eq!(x.err(), Some("Nothing here"));
  /// </code>
  /// <example>
  /// </summary>
  /// <returns>The <c>Err</c> value of <c>self</c>, wrapped in 
  /// and <c>Option<E></c></returns>
  public Result<T, E> Err()
  {
    return Result<T, E>.Err(_err!);
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
  /// Panics
  ///
  /// Panics if the value is an <c>Err</c>, with a panic message including the
  /// passed message, and the content of the <c>Err</c>.
  ///
  /// <example>
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<u32, &str> = Err("emergency failure");
  /// x.expect("Testing expect"); // panics with <c>Testing expect: emergency failure<c>
  /// </code>
  /// </example>
  ///
  /// Recommended Message Style
  ///
  /// We recommend that <c>expect</c> messages are used to describe the reason you
  /// <i>expect</i> the <c>Result</c> should be <c>Ok</c>.
  ///
  /// <code>
  /// let path = std::env::var("IMPORTANT_PATH")
  ///     .expect("env variable <c>IMPORTANT_PATH<c> should be set by <c>wrapper_script.sh<c>");
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
  /// <returns>The unwrapped value of <c>self</c></returns>
  public T Expect(string msg)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Err</c> value, consuming the <c>self<c> value.
  ///
  /// # Panics
  ///
  /// Panics if the value is an <c>Ok</c>, with a panic message including the
  /// passed message, and the content of the <c>Ok</c>.
  ///
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>should_panic
  /// let x: Result<u32, &str> = Ok(10);
  /// x.expect_err("Testing expect_err"); // panics with <c>Testing expect_err: 10<c>
  /// <code>
  /// </summary>
  /// <param name="msg"></param>
  /// <returns></returns>
  public E ExpectErr(string msg)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns <c>true<c> if the result is <c>Err</c>.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<i32, &str> = Ok(-3);
  /// assert_eq!(x.is_err(), false);
  ///
  /// let x: Result<i32, &str> = Err("Some error message");
  /// assert_eq!(x.is_err(), true);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public bool IsErr()
  {
    return this.Kind == ResultKind.Err;
  }

  /// <summary>
  /// Returns <c>true<c> if the result is <c>Ok</c>.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<i32, &str> = Ok(-3);
  /// assert_eq!(x.is_ok(), true);
  ///
  /// let x: Result<i32, &str> = Err("Some error message");
  /// assert_eq!(x.is_ok(), false);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public bool IsOk()
  {
    return this.Kind == ResultKind.Ok;
  }

  /// <summary>
  /// Returns an iterator over the possibly contained value.
  ///
  /// The iterator yields one value if the result is <c>Result::Ok</c>, otherwise none.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<u32, &str> = Ok(7);
  /// assert_eq!(x.iter().next(), Some(&7));
  ///
  /// let x: Result<u32, &str> = Err("nothing!");
  /// assert_eq!(x.iter().next(), None);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public IEnumerable<Option<T>> Iter()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Maps a <c>Result<T, E><c> to <c>Result<U, E><c> by applying a function to a
  /// contained <c>Ok</c> value, leaving an <c>Err</c> value untouched.
  ///
  /// This function can be used to compose the results of two functions.
  ///
  /// Examples
  ///
  /// Print the numbers on each line of a string multiplied by two.
  ///
  /// <code>
  /// let line = "1\n2\n3\n4\n";
  ///
  /// for num in line.lines() {
  ///     match num.parse::<i32>().map(|i| i * 2) {
  ///         Ok(n) => println!("{n}"),
  ///         Err(..) => {}
  ///     }
  /// }
  /// <code>
  /// </summary>
  /// <param name="op"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public Result<U, E> Map<U>(Func<T, U> op)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Maps a <c>Result<T, E><c> to <c>Result<T, F><c> by applying a function to a
  /// contained <c>Err</c> value, leaving an <c>Ok</c> value untouched.
  ///
  /// This function can be used to pass through a successful result while handling
  /// an error.
  ///
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// fn stringify(x: u32) -> String { format!("error code: {x}") }
  ///
  /// let x: Result<u32, u32> = Ok(2);
  /// assert_eq!(x.map_err(stringify), Ok(2));
  ///
  /// let x: Result<u32, u32> = Err(13);
  /// assert_eq!(x.map_err(stringify), Err("error code: 13".to_string()));
  /// <code>
  /// </summary>
  /// <param name="op"></param>
  /// <typeparam name="F"></typeparam>
  /// <returns></returns>
  public Result<T, F> MapErr<F>(Func<E, F> op)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the provided default (if <c>Err</c>), or
  /// applies a function to the contained value (if <c>Ok</c>),
  ///
  /// Arguments passed to <c>map_or<c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>map_or_else</c>,
  /// which is lazily evaluated.
  ///
  /// <c>map_or_else</c>: Result::map_or_else
  ///
  /// Examples
  ///
  /// <code>
  /// let x: Result<_, &str> = Ok("foo");
  /// assert_eq!(x.map_or(42, |v| v.len()), 3);
  ///
  /// let x: Result<&str, _> = Err("bar");
  /// assert_eq!(x.map_or(42, |v| v.len()), 42);
  /// <code>
  /// </summary>
  /// <param name="def"></param>
  /// <param name="f"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public U MapOr<U>(U def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Maps a <c>Result<T, E><c> to <c>U<c> by applying fallback function <c>default<c> to
  /// a contained <c>Err</c> value, or function <c>f<c> to a contained <c>Ok</c> value.
  ///
  /// This function can be used to unpack a successful result
  /// while handling an error.
  ///
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let k = 21;
  ///
  /// let x : Result<_, &str> = Ok("foo");
  /// assert_eq!(x.map_or_else(|e| k * 2, |v| v.len()), 3);
  ///
  /// let x : Result<&str, _> = Err("bar");
  /// assert_eq!(x.map_or_else(|e| k * 2, |v| v.len()), 42);
  /// <code>
  /// </summary>
  /// <param name="def"></param>
  /// <param name="f"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public U MapOrElse<U>(Func<E, U> def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Converts from <c>Result<T, E><c> to <c>Option<T></c>.
  ///
  /// Converts <c>self<c> into an <c>Option<T></c>, consuming <c>self<c>,
  /// and discarding the error, if any.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.ok(), Some(2));
  ///
  /// let x: Result<u32, &str> = Err("Nothing here");
  /// assert_eq!(x.ok(), None);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public Result<T, E> Ok()
  {
    return Result<T, E>.Ok(_value!);
  }

  /// <summary>
  /// Returns <c>res<c> if the result is <c>Err</c>, otherwise returns the <c>Ok</c> value of <c>self<c>.
  ///
  /// Arguments passed to <c>or<c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use <c>or_else</c>, which is
  /// lazily evaluated.
  ///
  /// <c>or_else</c>: Result::or_else
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<u32, &str> = Ok(2);
  /// let y: Result<u32, &str> = Err("late error");
  /// assert_eq!(x.or(y), Ok(2));
  ///
  /// let x: Result<u32, &str> = Err("early error");
  /// let y: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.or(y), Ok(2));
  ///
  /// let x: Result<u32, &str> = Err("not a 2");
  /// let y: Result<u32, &str> = Err("late error");
  /// assert_eq!(x.or(y), Err("late error"));
  ///
  /// let x: Result<u32, &str> = Ok(2);
  /// let y: Result<u32, &str> = Ok(100);
  /// assert_eq!(x.or(y), Ok(2));
  /// <code>
  /// </summary>
  /// <param name="res"></param>
  /// <typeparam name="F"></typeparam>
  /// <returns></returns>
  public Result<T, F> Or<F>(Result<T, F> res)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Calls <c>op<c> if the result is <c>Err</c>, otherwise returns the <c>Ok</c> value of <c>self<c>.
  ///
  /// This function can be used for control flow based on result values.
  ///
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// fn sq(x: u32) -> Result<u32, u32> { Ok(x * x) }
  /// fn err(x: u32) -> Result<u32, u32> { Err(x) }
  ///
  /// assert_eq!(Ok(2).or_else(sq).or_else(sq), Ok(2));
  /// assert_eq!(Ok(2).or_else(err).or_else(sq), Ok(2));
  /// assert_eq!(Err(3).or_else(sq).or_else(err), Ok(9));
  /// assert_eq!(Err(3).or_else(err).or_else(err), Err(3));
  /// <code>
  /// </summary>
  /// <param name="op"></param>
  /// <typeparam name="F"></typeparam>
  /// <returns></returns>
  public Result<T, F> OrElse<F>(Func<E, Result<T, F>> op)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Transposes a <c>Result<c> of an <c>Option<c> into an <c>Option<c> of a <c>Result<c>.
  ///
  /// <c>Ok(None)<c> will be mapped to <c>None<c>.
  /// <c>Ok(Some(_))<c> and <c>Err(_)<c> will be mapped to <c>Some(Ok(_))<c> and <c>Some(Err(_))<c>.
  ///
  /// Examples
  ///
  /// <code>
  /// #[derive(Debug, Eq, PartialEq)]
  /// struct SomeErr;
  ///
  /// let x: Result<Option<i32>, SomeErr> = Ok(Some(5));
  /// let y: Option<Result<i32, SomeErr>> = Some(Ok(5));
  /// assert_eq!(x.transpose(), y);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public Option<Result<T, E>> Transpose()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value, consuming the <c>self<c> value.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the <c>Err</c>
  /// case explicitly, or call <c>unwrap_or</c>, <c>unwrap_or_else</c>, or
  /// <c>unwrap_or_default</c>.
  ///
  /// <c>unwrap_or</c>: Result::unwrap_or
  /// <c>unwrap_or_else</c>: Result::unwrap_or_else
  /// <c>unwrap_or_default</c>: Result::unwrap_or_default
  ///
  /// # Panics
  ///
  /// Panics if the value is an <c>Err</c>, with a panic message provided by the
  /// <c>Err</c>'s value.
  ///
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let x: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.unwrap(), 2);
  /// <code>
  ///
  /// <code>should_panic
  /// let x: Result<u32, &str> = Err("emergency failure");
  /// x.unwrap(); // panics with <c>emergency failure<c>
  /// <code>
  /// </summary>
  /// <returns></returns>
  public T Unwrap()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Err</c> value, consuming the <c>self<c> value.
  ///
  /// # Panics
  ///
  /// Panics if the value is an <c>Ok</c>, with a custom panic message provided
  /// by the <c>Ok</c>'s value.
  ///
  /// Examples
  ///
  /// <code>should_panic
  /// let x: Result<u32, &str> = Ok(2);
  /// x.unwrap_err(); // panics with <c>2<c>
  /// <code>
  ///
  /// <code>
  /// let x: Result<u32, &str> = Err("emergency failure");
  /// assert_eq!(x.unwrap_err(), "emergency failure");
  /// <code>
  /// </summary>
  /// <returns></returns>
  public E UnwrapErr()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value or a provided default.
  ///
  /// Arguments passed to <c>unwrap_or<c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>unwrap_or_else</c>,
  /// which is lazily evaluated.
  ///
  /// <c>unwrap_or_else</c>: Result::unwrap_or_else
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// let default = 2;
  /// let x: Result<u32, &str> = Ok(9);
  /// assert_eq!(x.unwrap_or(default), 9);
  ///
  /// let x: Result<u32, &str> = Err("error");
  /// assert_eq!(x.unwrap_or(default), default);
  /// <code>
  /// </summary>
  /// <param name="def"></param>
  /// <returns></returns>
  public T UnwrapOr(T def)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value or a default
  ///
  /// Consumes the <c>self<c> argument then, if <c>Ok</c>, returns the contained
  /// value, otherwise if <c>Err</c>, returns the default value for that
  /// type.
  ///
  /// Examples
  ///
  /// Converts a string to an integer, turning poorly-formed strings
  /// into 0 (the default value for integers). <c>parse</c> converts
  /// a string to any other type that implements <c>FromStr</c>, returning an
  /// <c>Err</c> on error.
  ///
  /// <code>
  /// let good_year_from_input = "1909";
  /// let bad_year_from_input = "190blarg";
  /// let good_year = good_year_from_input.parse().unwrap_or_default();
  /// let bad_year = bad_year_from_input.parse().unwrap_or_default();
  ///
  /// assert_eq!(1909, good_year);
  /// assert_eq!(0, bad_year);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public T UnwrapOrDefault()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Ok</c> value or computes it from a closure.
  ///
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// fn count(x: &str) -> usize { x.len() }
  ///
  /// assert_eq!(Ok(2).unwrap_or_else(count), 2);
  /// assert_eq!(Err("foo").unwrap_or_else(count), 3);
  /// <code>
  /// </summary>
  /// <param name="op"></param>
  /// <returns></returns>
  public T UnwrapOrElse(Func<E, T> op)
  {
    throw new NotImplementedException();
  }
}

#if TEST
public class ResultTests
{
  [Fact]
  public void DummyTest()
  {
    Assert.True(true);
  }

  [Fact]
  public void AndEarlyErrorTest()
  {
    var x = Result<int, string>.Err("early error");
    var y = Result<string, string>.Ok("foo");
    Assert.Equal(x.And<string>(y), Result<string, string>.Err("early error"));
  }

  [Fact]
  public void AndLateErrorTest()
  {
    var x = Result<int, string>.Ok(2);
    var y = Result<string, string>.Err("late error");
    Assert.Equal(x.And<string>(y), Result<string, string>.Err("late error"));
  }
}
#endif