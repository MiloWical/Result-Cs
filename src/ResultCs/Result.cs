namespace WicalWare.Components.ResultCs;

// TODO: Reread the comments and correct the formatting to C# styling!
// TODO: Check the <return> tags on all comments!

// https://doc.rust-lang.org/std/result/enum.Result.html
public class Result<T, E>
{
  // Imp Test Sig
  //          Result<U, E> And<U>(Result<U, E> optB)
  //          Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  //          Option<E> Err()
  //          T Expect(string msg)
  //          E ExpectErr(string msg)
  //          bool IsErr()
  //          bool IsOk()
  //          IEnumerable<Option<T>> Iter()
  //          Result<U, E> Map<U>(Func<T, U> op)
  //          Result<T, F> MapErr<F>(Func<E, F> op)
  //          U MapOr<U>(U def, Func<T, U> f)
  //          U MapOrElse<U>(Func<E, U> def, Func<T, U> f)
  //          Option<T> Ok()
  //          Result<T, F> Or<F>(Result<T, F> res)
  //          Result<T, F> OrElse<F>(Func<E, Result<T, F>> op) 
  //          Option<Result<T, E>> Transpose()
  //          T Unwrap()
  //          E UnwrapErr()
  //          T UnwrapOr(T def)
  //          T UnwrapOrDefault()
  //          T UnwrapOrElse(Func<E, T> op)
  //
  // Experimental signatures (not implemented)
  //          bool Contains<U>(U x)
  //          bool ContainsErr<F>(F f)
  //          Result<T, E> Flatten()
  //          Result<T, E> Inspect(Action<T> f)
  //          Result<T, E> InspectErr(Action<E> f)
  //          E IntoErr()
  //          T IntoOk()
  //          bool IsErrAnd(Func<E, bool> f)
  //          bool IsOkAnd(Func<T, bool> f)

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

  // TODO: Fix this up for this type!

  // public override bool Equals(object? other)
  // {
  //   if(other == null) ArgumentNullException.ThrowIfNull(other);

  //   var otherOption = (Option<T>) other;

  //   if(this.Kind != otherOption.Kind)
  //     return false;

  //   if(this.IsNone() && otherOption.IsNone())
  //     return true;

  //   if(_value == null)
  //   {
  //     if(otherOption.Unwrap() == null)
  //       return true;

  //     return false;
  //   }

  //   if(_value.Equals(otherOption.Unwrap()))
  //     return true;

  //   return false;
  // }

  // public static bool operator == (Option<T> opt1, Option<T> opt2) => opt1.Equals(opt2);

  // public static bool operator != (Option<T> opt1, Option<T> opt2) => !(opt1.Equals(opt2));

  // public override int GetHashCode() => ((object) this).GetHashCode(); 

  /// <summary>
  /// Returns [`None`] if the option is [`None`], otherwise returns `optb`.
  ///
  /// Arguments passed to `and` are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use [`and_then`], which is
  /// lazily evaluated.
  ///
  /// [`and_then`]: Option::and_then
  ///
  /// # Examples
  ///
  /// ```
  /// let x = Some(2);
  /// let y: Option<&str> = None;
  /// assert_eq!(x.and(y), None);
  ///
  /// let x: Option<u32> = None;
  /// let y = Some("foo");
  /// assert_eq!(x.and(y), None);
  ///
  /// let x = Some(2);
  /// let y = Some("foo");
  /// assert_eq!(x.and(y), Some("foo"));
  ///
  /// let x: Option<u32> = None;
  /// let y: Option<&str> = None;
  /// assert_eq!(x.and(y), None);
  /// ```
  /// </summary>
  /// <param name="optB"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public Result<U, E> And<U>(Result<U, E> optB)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns [`None`] if the option is [`None`], otherwise calls `f` with the
  /// wrapped value and returns the result.
  ///
  /// Some languages call this operation flatmap.
  ///
  /// # Examples
  ///
  /// ```
  /// fn sq_then_to_string(x: u32) -> Option<String> {
  ///     x.checked_mul(x).map(|sq| sq.to_string())
  /// }
  ///
  /// assert_eq!(Some(2).and_then(sq_then_to_string), Some(4.to_string()));
  /// assert_eq!(Some(1_000_000).and_then(sq_then_to_string), None); // overflowed!
  /// assert_eq!(None.and_then(sq_then_to_string), None);
  /// ```
  ///
  /// Often used to chain fallible operations that may return [`None`].
  ///
  /// ```
  /// let arr_2d = [["A0", "A1"], ["B0", "B1"]];
  ///
  /// let item_0_1 = arr_2d.get(0).and_then(|row| row.get(1));
  /// assert_eq!(item_0_1, Some(&"A1"));
  ///
  /// let item_2_0 = arr_2d.get(2).and_then(|row| row.get(0));
  /// assert_eq!(item_2_0, None);
  /// ```
  /// </summary>
  /// <param name="f"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public Result<U, E> AndThen<U>(Func<T, Result<U, E>> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Converts from `Result<T, E>` to [`Option<E>`].
  ///
  /// Converts `self` into an [`Option<E>`], consuming `self`,
  /// and discarding the success value, if any.
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let x: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.err(), None);
  ///
  /// let x: Result<u32, &str> = Err("Nothing here");
  /// assert_eq!(x.err(), Some("Nothing here"));
  /// ```
  /// </summary>
  /// <returns></returns>
  public Option<E> Err()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Ok`] value, consuming the `self` value.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the [`Err`]
  /// case explicitly, or call [`unwrap_or`], [`unwrap_or_else`], or
  /// [`unwrap_or_default`].
  ///
  /// [`unwrap_or`]: Result::unwrap_or
  /// [`unwrap_or_else`]: Result::unwrap_or_else
  /// [`unwrap_or_default`]: Result::unwrap_or_default
  ///
  /// # Panics
  ///
  /// Panics if the value is an [`Err`], with a panic message including the
  /// passed message, and the content of the [`Err`].
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```should_panic
  /// let x: Result<u32, &str> = Err("emergency failure");
  /// x.expect("Testing expect"); // panics with `Testing expect: emergency failure`
  /// ```
  ///
  /// # Recommended Message Style
  ///
  /// We recommend that `expect` messages are used to describe the reason you
  /// _expect_ the `Result` should be `Ok`.
  ///
  /// ```should_panic
  /// let path = std::env::var("IMPORTANT_PATH")
  ///     .expect("env variable `IMPORTANT_PATH` should be set by `wrapper_script.sh`");
  /// ```
  ///
  /// **Hint**: If you're having trouble remembering how to phrase expect
  /// error messages remember to focus on the word "should" as in "env
  /// variable should be set by blah" or "the given binary should be available
  /// and executable by the current user".
  ///
  /// For more detail on expect message styles and the reasoning behind our recommendation please
  /// refer to the section on ["Common Message
  /// Styles"](../../std/error/index.html#common-message-styles) in the
  /// [`std::error`](../../std/error/index.html) module docs.
  /// </summary>
  /// <param name="msg"></param>
  /// <returns></returns>
  public T Expect(string msg)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Err`] value, consuming the `self` value.
  ///
  /// # Panics
  ///
  /// Panics if the value is an [`Ok`], with a panic message including the
  /// passed message, and the content of the [`Ok`].
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```should_panic
  /// let x: Result<u32, &str> = Ok(10);
  /// x.expect_err("Testing expect_err"); // panics with `Testing expect_err: 10`
  /// ```
  /// </summary>
  /// <param name="msg"></param>
  /// <returns></returns>
  public E ExpectErr(string msg)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns `true` if the result is [`Err`].
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let x: Result<i32, &str> = Ok(-3);
  /// assert_eq!(x.is_err(), false);
  ///
  /// let x: Result<i32, &str> = Err("Some error message");
  /// assert_eq!(x.is_err(), true);
  /// ```
  /// </summary>
  /// <returns></returns>
  public bool IsErr()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns `true` if the result is [`Ok`].
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let x: Result<i32, &str> = Ok(-3);
  /// assert_eq!(x.is_ok(), true);
  ///
  /// let x: Result<i32, &str> = Err("Some error message");
  /// assert_eq!(x.is_ok(), false);
  /// ```
  /// </summary>
  /// <returns></returns>
  public bool IsOk()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns an iterator over the possibly contained value.
  ///
  /// The iterator yields one value if the result is [`Result::Ok`], otherwise none.
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let x: Result<u32, &str> = Ok(7);
  /// assert_eq!(x.iter().next(), Some(&7));
  ///
  /// let x: Result<u32, &str> = Err("nothing!");
  /// assert_eq!(x.iter().next(), None);
  /// ```
  /// </summary>
  /// <returns></returns>
  public IEnumerable<Option<T>> Iter()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Maps a `Result<T, E>` to `Result<U, E>` by applying a function to a
  /// contained [`Ok`] value, leaving an [`Err`] value untouched.
  ///
  /// This function can be used to compose the results of two functions.
  ///
  /// # Examples
  ///
  /// Print the numbers on each line of a string multiplied by two.
  ///
  /// ```
  /// let line = "1\n2\n3\n4\n";
  ///
  /// for num in line.lines() {
  ///     match num.parse::<i32>().map(|i| i * 2) {
  ///         Ok(n) => println!("{n}"),
  ///         Err(..) => {}
  ///     }
  /// }
  /// ```
  /// </summary>
  /// <param name="op"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public Result<U, E> Map<U>(Func<T, U> op)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Maps a `Result<T, E>` to `Result<T, F>` by applying a function to a
  /// contained [`Err`] value, leaving an [`Ok`] value untouched.
  ///
  /// This function can be used to pass through a successful result while handling
  /// an error.
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// fn stringify(x: u32) -> String { format!("error code: {x}") }
  ///
  /// let x: Result<u32, u32> = Ok(2);
  /// assert_eq!(x.map_err(stringify), Ok(2));
  ///
  /// let x: Result<u32, u32> = Err(13);
  /// assert_eq!(x.map_err(stringify), Err("error code: 13".to_string()));
  /// ```
  /// </summary>
  /// <param name="op"></param>
  /// <typeparam name="F"></typeparam>
  /// <returns></returns>
  public Result<T, F> MapErr<F>(Func<E, F> op)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the provided default (if [`Err`]), or
  /// applies a function to the contained value (if [`Ok`]),
  ///
  /// Arguments passed to `map_or` are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use [`map_or_else`],
  /// which is lazily evaluated.
  ///
  /// [`map_or_else`]: Result::map_or_else
  ///
  /// # Examples
  ///
  /// ```
  /// let x: Result<_, &str> = Ok("foo");
  /// assert_eq!(x.map_or(42, |v| v.len()), 3);
  ///
  /// let x: Result<&str, _> = Err("bar");
  /// assert_eq!(x.map_or(42, |v| v.len()), 42);
  /// ```
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
  /// Maps a `Result<T, E>` to `U` by applying fallback function `default` to
  /// a contained [`Err`] value, or function `f` to a contained [`Ok`] value.
  ///
  /// This function can be used to unpack a successful result
  /// while handling an error.
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let k = 21;
  ///
  /// let x : Result<_, &str> = Ok("foo");
  /// assert_eq!(x.map_or_else(|e| k * 2, |v| v.len()), 3);
  ///
  /// let x : Result<&str, _> = Err("bar");
  /// assert_eq!(x.map_or_else(|e| k * 2, |v| v.len()), 42);
  /// ```
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
  /// Converts from `Result<T, E>` to [`Option<T>`].
  ///
  /// Converts `self` into an [`Option<T>`], consuming `self`,
  /// and discarding the error, if any.
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let x: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.ok(), Some(2));
  ///
  /// let x: Result<u32, &str> = Err("Nothing here");
  /// assert_eq!(x.ok(), None);
  /// ```
  /// </summary>
  /// <returns></returns>
  public Option<T> Ok()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns `res` if the result is [`Err`], otherwise returns the [`Ok`] value of `self`.
  ///
  /// Arguments passed to `or` are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use [`or_else`], which is
  /// lazily evaluated.
  ///
  /// [`or_else`]: Result::or_else
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
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
  /// ```
  /// </summary>
  /// <param name="res"></param>
  /// <typeparam name="F"></typeparam>
  /// <returns></returns>
  public Result<T, F> Or<F>(Result<T, F> res)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Calls `op` if the result is [`Err`], otherwise returns the [`Ok`] value of `self`.
  ///
  /// This function can be used for control flow based on result values.
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// fn sq(x: u32) -> Result<u32, u32> { Ok(x * x) }
  /// fn err(x: u32) -> Result<u32, u32> { Err(x) }
  ///
  /// assert_eq!(Ok(2).or_else(sq).or_else(sq), Ok(2));
  /// assert_eq!(Ok(2).or_else(err).or_else(sq), Ok(2));
  /// assert_eq!(Err(3).or_else(sq).or_else(err), Ok(9));
  /// assert_eq!(Err(3).or_else(err).or_else(err), Err(3));
  /// ```
  /// </summary>
  /// <param name="op"></param>
  /// <typeparam name="F"></typeparam>
  /// <returns></returns>
  public Result<T, F> OrElse<F>(Func<E, Result<T, F>> op)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Transposes a `Result` of an `Option` into an `Option` of a `Result`.
  ///
  /// `Ok(None)` will be mapped to `None`.
  /// `Ok(Some(_))` and `Err(_)` will be mapped to `Some(Ok(_))` and `Some(Err(_))`.
  ///
  /// # Examples
  ///
  /// ```
  /// #[derive(Debug, Eq, PartialEq)]
  /// struct SomeErr;
  ///
  /// let x: Result<Option<i32>, SomeErr> = Ok(Some(5));
  /// let y: Option<Result<i32, SomeErr>> = Some(Ok(5));
  /// assert_eq!(x.transpose(), y);
  /// ```
  /// </summary>
  /// <returns></returns>
  public Option<Result<T, E>> Transpose()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Ok`] value, consuming the `self` value.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the [`Err`]
  /// case explicitly, or call [`unwrap_or`], [`unwrap_or_else`], or
  /// [`unwrap_or_default`].
  ///
  /// [`unwrap_or`]: Result::unwrap_or
  /// [`unwrap_or_else`]: Result::unwrap_or_else
  /// [`unwrap_or_default`]: Result::unwrap_or_default
  ///
  /// # Panics
  ///
  /// Panics if the value is an [`Err`], with a panic message provided by the
  /// [`Err`]'s value.
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let x: Result<u32, &str> = Ok(2);
  /// assert_eq!(x.unwrap(), 2);
  /// ```
  ///
  /// ```should_panic
  /// let x: Result<u32, &str> = Err("emergency failure");
  /// x.unwrap(); // panics with `emergency failure`
  /// ```
  /// </summary>
  /// <returns></returns>
  public T Unwrap()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Err`] value, consuming the `self` value.
  ///
  /// # Panics
  ///
  /// Panics if the value is an [`Ok`], with a custom panic message provided
  /// by the [`Ok`]'s value.
  ///
  /// # Examples
  ///
  /// ```should_panic
  /// let x: Result<u32, &str> = Ok(2);
  /// x.unwrap_err(); // panics with `2`
  /// ```
  ///
  /// ```
  /// let x: Result<u32, &str> = Err("emergency failure");
  /// assert_eq!(x.unwrap_err(), "emergency failure");
  /// ```
  /// </summary>
  /// <returns></returns>
  public E UnwrapErr()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Ok`] value or a provided default.
  ///
  /// Arguments passed to `unwrap_or` are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use [`unwrap_or_else`],
  /// which is lazily evaluated.
  ///
  /// [`unwrap_or_else`]: Result::unwrap_or_else
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// let default = 2;
  /// let x: Result<u32, &str> = Ok(9);
  /// assert_eq!(x.unwrap_or(default), 9);
  ///
  /// let x: Result<u32, &str> = Err("error");
  /// assert_eq!(x.unwrap_or(default), default);
  /// ```
  /// </summary>
  /// <param name="def"></param>
  /// <returns></returns>
  public T UnwrapOr(T def)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Ok`] value or a default
  ///
  /// Consumes the `self` argument then, if [`Ok`], returns the contained
  /// value, otherwise if [`Err`], returns the default value for that
  /// type.
  ///
  /// # Examples
  ///
  /// Converts a string to an integer, turning poorly-formed strings
  /// into 0 (the default value for integers). [`parse`] converts
  /// a string to any other type that implements [`FromStr`], returning an
  /// [`Err`] on error.
  ///
  /// ```
  /// let good_year_from_input = "1909";
  /// let bad_year_from_input = "190blarg";
  /// let good_year = good_year_from_input.parse().unwrap_or_default();
  /// let bad_year = bad_year_from_input.parse().unwrap_or_default();
  ///
  /// assert_eq!(1909, good_year);
  /// assert_eq!(0, bad_year);
  /// ```
  /// </summary>
  /// <returns></returns>
  public T UnwrapOrDefault()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained [`Ok`] value or computes it from a closure.
  ///
  ///
  /// # Examples
  ///
  /// Basic usage:
  ///
  /// ```
  /// fn count(x: &str) -> usize { x.len() }
  ///
  /// assert_eq!(Ok(2).unwrap_or_else(count), 2);
  /// assert_eq!(Err("foo").unwrap_or_else(count), 3);
  /// ```
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
}
#endif