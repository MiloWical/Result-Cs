namespace WicalWare.Components.ResultCs;

// https://doc.rust-lang.org/std/option/enum.Option.html
public class Option<T>
{
  // Comment Imp Test Sig
  // ✓       ✓   ✓    Option<U> And<U>(Option<U> optB)
  // ✓       ✓   ✓    Option<U> AndThen<U>(Func<T, Option<U>> f)
  // ✓       ✓   ✓    T Expect(string message)
  // ✓       ✓   ✓    Option<T> Filter(Func<T, bool> predicate)
  // ✓       ✓   ✓    Option<T> Flatten()
  // ✓       ✓   ✓    T GetOrInsert(T value)
  //                  T GetOrInsertWith(Func<T> f)
  //                  T Insert(T value)
  // ✓       ✓   ✓    bool IsNone()
  // ✓       ✓   ✓    bool IsSome()
  //                  bool IsSomeAnd(Func<T, bool> f)
  //                  IEnumerable<Option<T>> Iter()
  //                  Option<U> Map<U>(Func<T, U> f)
  //                  U MapOr<U>(U def, Func<T, U> f)
  //                  U MapOrElse<U>(Func<U> def, Func<T, U> f)
  //                  Result<T, E> OkOr<E>(E err)
  //                  Result<T, E> OkOrElse<E>(Func<E> err)
  //                  Option<T> Or(Option<T> optB)
  //                  Option<T> OrElse(Func<Option<T>> f)
  //                  Option<T> Replace(T value)
  //                  Option<T> Take()
  //                  Result<Option<T>, E> Transpose<E>()
  //         ✓   ✓    T Unwrap()
  //                  T UnwrapOr(T def)
  //                  T UnwrapOrDefault()
  //                  T UnwrapOrElse(Func<T> f)
  //                  Option<T> Xor(Option<T> optB)
  //                  Option<(T, U)> Zip<U>(Option<U> other)
  // ✓       ✓   ✓    OptionKind Kind { get; }
  //
  // Experimental signatures (not implemented)
  //                  bool Contains<U>(U x)
  //                  T GetOrInsertDefault()
  //                  Option<T> Inspect(Action<T> f)
  //                  bool IsSomeAnd(Func<T, bool> f)
  //                  (Option<T>, Option<U>) Unzip<U>()
  //                  Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> f)

  /// <summary>
  /// The <see cref="OptionKind"/> of the current <c>Option</c>.
  /// </summary>
  public OptionKind Kind { get; private set; }

  // If this Option is Some, the value wrapped by the response.
  private T? _value;

  /// <summary>
  /// Generates an <c>Option</c> with an <c>OptionKind</c> of <see cref="OptionKind.Some"/>.
  /// 
  /// The value passed is not permitted to be <c>null</c>.
  /// </summary>
  /// <param name="value">The value to wrap in the <c>Option</c></param>
  /// <returns>The <c>Option</c> of kind <c>Some</c> with the provided value wrapped.</returns>
  public static Option<T> Some(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    var option = new Option<T>();
    option._value = value;
    option.Kind = OptionKind.Some;

    return option;
  }

  /// <summary>
  /// Generates an <c>Option</c> with an <c>OptionKind</c> of <see cref="OptionKind.None"/>.
  /// 
  /// The value passed is not permitted to be <c>null</c>.
  /// </summary>
  /// <returns>The <c>Option</c> of kind <c>None</c>.</returns>
  public static Option<T> None()
  {
    return new Option<T>
    {
      Kind = OptionKind.None
    };
  }

  /// <summary>
  /// Tests two <c>Option</c> objects for equality.
  /// 
  /// Overrides <see cref="Object.Equals(object?)"/>.
  /// </summary>
  /// <param name="other">The <c>Option</c> to test <c>this</c> against.</param>
  /// <returns><c>true</c> if one of the following is true:
  /// <ul>
  /// <li>The two <c>Option</c> objects are <c>Some</c> and their wrapped values are the same</li>
  /// <li>The two <c>Option</c> objects are <c>None</c></li>
  /// </ul>
  /// <c>false</c> otherwise</returns>
  public override bool Equals(object? other)
  {
    if (other == null) ArgumentNullException.ThrowIfNull(other);

    var otherOption = (Option<T>)other;

    if (this.Kind != otherOption.Kind)
      return false;

    if (this.IsNone() && otherOption.IsNone())
      return true;

    if (_value == null)
    {
      if (otherOption.Unwrap() == null)
        return true;

      return false;
    }

    if (_value.Equals(otherOption.Unwrap()))
      return true;

    return false;
  }

  /// <summary>
  /// Convenience override of the <c>==</c> operator.
  /// </summary>
  /// <param name="opt1">The first <c>Option</c> to compare</param>
  /// <param name="opt2">The second <c>Option</c> to compare</param>
  /// <returns>The result of <c>opt1.Equals(opt2)</c></returns>
  public static bool operator ==(Option<T> opt1, Option<T> opt2) => opt1.Equals(opt2);

  /// <summary>
  /// Convenience override of the <c>!=</c> operator.
  /// </summary>
  /// <param name="opt1">The first <c>Option</c> to compare</param>
  /// <param name="opt2">The second <c>Option</c> to compare</param>
  /// <returns>The result of <c>!opt1.Equals(opt2)</c></returns>
  public static bool operator !=(Option<T> opt1, Option<T> opt2) => !(opt1.Equals(opt2));

  /// <summary>
  /// Override of <see cref="Object.GetHashCode()"/>.
  /// </summary>
  /// <returns><c>base.GetHashCode()</c></returns>
  public override int GetHashCode() => base.GetHashCode();

  /// <summary>
  /// Arguments passed to <c>And</c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use 
  /// <c><see cref="AndThen"></c></see>, which is lazily evaluated.
  ///
  /// <example>
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.Some(2);
  /// var y = Option<string>.None();
  /// Assert.True(x.And<string>(y).IsNone());
  ///
  /// var x = Option<int>.None();
  /// var y = Option<string>.Some("foo");
  /// Assert.True(x.And<string>(y).IsNone());
  ///
  /// var x = Option<int>.Some(2);
  /// var y = Option<string>.Some("foo");
  /// Assert.Equal(x.And<string>(y), Option<string>.Some("foo"));
  ///
  /// var x = Option<int>.None();
  /// var y = Option<string>.None();
  /// Assert.True(x.And<string>(y).IsNone());
  /// </code>
  /// </example>
  /// </summary>
  /// <param name="optB">The option to return if the current option is <c>None</c></param>
  /// <typeparam name="U">The type of <c>optB</c></typeparam>
  /// <returns>Returns <c>None</c>if the option is <c>None</c>, 
  /// otherwise returns <c>optb</c>.</returns>
  public Option<U> And<U>(Option<U> optB)
  {
    if (this.IsNone())
      return Option<U>.None();

    return optB;
  }

  /// <summary>
  /// Checks <c>self</c> to see if it's <c>Some</c>, then calls <c>f</c>
  /// on the result and returns the returned <c>Option</c>.
  /// 
  /// Some languages call this operation flatmap.
  ///
  /// <example>
  /// Examples
  ///
  /// <code>
  /// public Option<string> SqThenToString(int x)
  /// {
  ///   return Option<string>.Some((x * x).ToString());
  /// }
  ///
  /// Assert.Equal(Option<int>.Some(2).AndThen(SqThenToString), Option<string>.Some(4.ToString()));
  /// 
  /// Assert.Equal(Option<int>.Some(1_000_000).AndThen(SqThenToString), Option<string>.None()); //throws OverflowException
  /// Assert.Equal(Option<int>.None().AndThen(SqThenToString), Option<string>.None());
  /// </code>
  ///
  /// Often used to chain fallible operations that may return <c>None</c>.
  ///
  /// <code>
  /// var arr_2D = new string[2,2]{{"A0", "A1"}, {"B0", "B1"}};
  ///
  /// var item_0_1 = GetRow(arr_2D, 0).AndThen(row => GetElement(row, 1));
  /// Assert.Equal(Option<string>.Some("A1"), item_0_1);
  ///
  /// var item_2_0 = GetRow(arr_2D, 2).AndThen(row => GetElement(row, 0));
  /// Assert.Equal(Option<string>.None(), item_2_0);
  /// </code>
  /// </example>
  /// </summary>
  /// <param name="f">The function to call with the unwrapped value of <c>self</c></param>
  /// <typeparam name="U">The underlying type of the result of <c>f</c></typeparam>
  /// <returns>Returns <c>None</c> if the option is <c>None</c>,
  /// otherwise calls <c>f</c> with the wrapped value and returns the result.</returns>
  public Option<U> AndThen<U>(Func<T, Option<U>> f)
  {
    if (this.IsNone())
      return Option<U>.None();

    return f.Invoke(this.Unwrap());
  }

  /// <summary>
  /// Returns the contained <c>Some</c> value, consuming the <c>self<c> value.
  ///
  /// # Panics
  ///
  /// Panics if the value is a <c>None</c> with a custom panic message provided by
  /// <c>msg<c>.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<string>.Some("value");
  /// Assert.Equal(x.Expect("fruits are healthy"), "value");
  /// <code>
  ///
  /// <code>
  /// var x = Option<string>.None();
  /// x.Expect("fruits are healthy"); // panics with <c>fruits are healthy<c>
  /// <code>
  ///
  /// # Recommended Message Style
  ///
  /// We recommend that <c>expect<c> messages are used to describe the reason you
  /// _expect_ the <c>Option<c> should be <c>Some<c>.
  ///
  /// <code>
  /// var slice = new int[0];
  /// var item = GetElement(slice, 0)
  ///     .Expect("slice should not be empty");
  /// <code>
  ///
  /// **Hint**: If you're having trouble remembering how to phrase expect
  /// error messages remember to focus on the word "should" as in "env
  /// variable should be set by blah" or "the given binary should be available
  /// and executable by the current user".
  ///
  /// For more detail on expect message styles and the reasoning behind our
  /// recommendation please refer to the section on ["Common Message
  /// Styles"](../../std/error/index.html#common-message-styles) in the <c>std::error</c>(../../std/error/index.html) module docs.
  /// </summary>
  /// <param name="message">The message to panic with if <c>this</c> is <c>None</c>.</param>
  /// <returns>The wrapped <c>Some</c> value.</returns>
  public T Expect(string message)
  {
    if (this.IsNone())
      throw new PanicException(message);

    return this.Unwrap();
  }

  /// <summary>
  /// Returns <c>None</c> if the option is <c>None</c>, otherwise calls <c>predicate<c>
  /// with the wrapped value and returns:
  ///
  /// - <c>Some(t)</c> if <c>predicate<c> returns <c>true<c> (where <c>t<c> is the wrapped
  ///   value), and
  /// - <c>None</c> if <c>predicate<c> returns <c>false<c>.
  ///
  /// You can imagine the <c>Option<T><c> being an iterator over one or zero elements. 
  /// <c>filter()<c> lets you decide which elements to keep.
  ///
  /// Examples
  ///
  /// <code>
  /// public bool IsEven(int n)
  /// {
  ///   return (n % 2) == 0;
  /// }
  ///
  /// Assert.Equal(Option<int>.None(), Option<int>.None().Filter(IsEven));
  /// Assert.Equal(Option<int>.None(), Option<int>.Some(3).Filter(IsEven));
  /// Assert.Equal(Option<int>.Some(4), Option<int>.Some(4).Filter(IsEven));
  /// <code>
  ///
  /// </summary>
  /// <param name="predicate">The function used to evaluate the value of <c>this</c>
  /// if it's <c>Some</c>.</param>
  /// <returns>The unwrapped value of <c>this</c> if
  /// <ul>
  /// <li><c>this</c> is <c>Some</c> AND </li>
  /// <li><c>predicate</c> returns <c>true</c>
  /// </ul>
  /// <c>false</c> otherwise.</returns>
  public Option<T> Filter(Func<T, bool> predicate)
  {
    if (this.IsNone())
      return Option<T>.None();

    if(predicate.Invoke(this.Unwrap()))
      return Option<T>.Some(this.Unwrap());

    return Option<T>.None();
  }

  /// <summary>
  /// Converts from <c>Option<Option<T>><c> to <c>Option<T><c>.
  ///
  /// Examples
  ///
  /// Basic usage:
  ///
  /// <code>
  /// var x = Option<Option<int>>.Some(Option<int>.Some(6));
  /// Assert.Equal(Option<int>.Some(6), x.Flatten());
  ///
  /// var x = Option<Option<int>>.Some(Option<int>.None());
  /// Assert.Equal(Option<int>.None(), x.Flatten());
  ///
  /// var x = Option<Option<int>>.None();
  /// Assert.Equal(Option<int>.None(), x.Flatten());
  /// <code>
  ///
  /// Flattening only removes one level of nesting at a time:
  ///
  /// <code>
  /// var x = Option<Option<Option<int>>>.Some(Option<Option<int>>.Some(Option<int>.Some(6)));
  /// Assert.Equal(Option<Option<int>>.Some(Option<int>.Some(6)), x.Flatten());
  /// Assert.Equal(Option<int>.Some(6), x.Flatten().Flatten());
  /// <code>
  /// </summary>
  /// <returns>The <c>Option</c> that's wrapped by <c>this</c>.</returns>
  public T Flatten()
  {
    if(this.IsNone() && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Option<>))
        return (T)(typeof(T).GetMethod("None")!.Invoke(null, new object[0]))!;

    return this.Unwrap();
  }

  /// <summary>
  /// Inserts <c>value<c> into the <c>Option</c> if it is <c>None</c> and returns
  /// the value stored.
  ///
  /// See also <c>Option.Insert</c>, which updates the value even if
  /// the option already contains <c>Some</c>.
  /// 
  /// Note: This differs from the original implementation because it doesn't
  /// return a mutable reference to the contained value due to differences in
  /// how the languages work.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.None();
  /// var y = x.GetOrInsert(5);
  /// 
  /// Assert.Equal(5, y);
  /// Assert.Equal(Option<int>.Some(5), x);
  /// <code>
  /// </summary>
  /// <param name="value">The value to insert if <c>this</c> is <c>None</c>.</param>
  /// <returns>The value contained in <c>this</c>.</returns>
  public T GetOrInsert(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    if(this.IsNone())
    {
      this._value = value;
      this.Kind = OptionKind.Some;
    }  

    return _value!;
  }

  /// <summary>
  /// Inserts a value computed from <c>f<c> into the option if it is <c>None</c>,
  /// then returns a mutable reference to the contained value.
  ///
  /// Examples
  ///
  /// <code>
  /// let mut x = None;
  ///
  /// {
  ///     let y: &mut int = x.get_or_insert_with(|| 5);
  ///     assert_eq!(y, &5);
  ///
  ///     *y = 7;
  /// }
  ///
  /// assert_eq!(x, Some(7));
  /// <code>
  /// </summary>
  /// <param name="f"></param>
  /// <returns></returns>
  public T GetOrInsertWith(Func<T> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Inserts <c>value<c> into the option, then returns a mutable reference to it.
  ///
  /// If the option already contains a value, the old value is dropped.
  ///
  /// See also <c>Option::get_or_insert</c>, which doesn't update the value if
  /// the option already contains <c>Some</c>.
  ///
  /// # Example
  ///
  /// <code>
  /// let mut opt = None;
  /// let val = opt.insert(1);
  /// assert_eq!(*val, 1);
  /// assert_eq!(opt.unwrap(), 1);
  /// let val = opt.insert(2);
  /// assert_eq!(*val, 2);
  /// *val = 3;
  /// assert_eq!(opt.unwrap(), 3);
  /// <code>
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public T Insert(T value)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns <c>true<c> if the option is a <c>None</c> value.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.Some(2);
  /// Assert.False(x.IsNone());
  ///
  /// var x = Option<int>.None();
  /// Assert.True(x.IsNone());
  /// <code>
  /// </summary>
  /// <returns><c>true</c> if <c>this</c> is <c>None</c>;
  /// <c>false</c> otherwise.</returns>
  public bool IsNone() => Kind == OptionKind.None;

  /// <summary>
  /// Returns <c>true<c> if the option is a <c>Some</c> value.
  ///
  /// Examples
  ///
  /// <code>
  /// Option<int> x = Option.Some(2);
  /// Assert.True(x.IsSome());
  ///
  /// Option<int> x = Option.None();
  /// Assert.False(x.IsSome());
  /// <code>
  /// </summary>
  /// <returns><c>true</c> if self is <c>Some</c>, <c>false</c> 
  /// if self is <c>None</c>.</returns>
  public bool IsSome() => Kind == OptionKind.Some;

  /// <summary>
  /// Returns <c>true<c> if the option is a <c>Some</c> and the value inside of it matches a predicate.
  ///
  /// Examples
  ///
  /// <code>
  /// #![feature(is_some_with)]
  ///
  /// let x: Option<int> = Some(2);
  /// assert_eq!(x.is_some_and(|&x| x > 1), true);
  ///
  /// let x: Option<int> = Some(0);
  /// assert_eq!(x.is_some_and(|&x| x > 1), false);
  ///
  /// let x: Option<int> = None;
  /// assert_eq!(x.is_some_and(|&x| x > 1), false);
  /// <code>
  /// </summary>
  /// <param name="f"></param>
  /// <returns></returns>
  public bool IsSomeAnd(Func<T, bool> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns an iterator over the possibly contained value.
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some(4);
  /// assert_eq!(x.iter().next(), Some(&4));
  ///
  /// let x: Option<int> = None;
  /// assert_eq!(x.iter().next(), None);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public IEnumerable<Option<T>> Iter()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Maps an <c>Option<T><c> to <c>Option<U><c> by applying a function to a contained value.
  ///
  /// Examples
  ///
  /// Converts an <code>Option<[String]></code> into an <code>Option<[usize]></code>, consuming
  /// the original:
  ///
  /// [String]: ../../std/string/struct.String.html "String"
  /// <code>
  /// let maybe_some_string = Some(String::from("Hello, World!"));
  /// // <c>Option::map<c> takes self *by value*, consuming <c>maybe_some_string<c>
  /// let maybe_some_len = maybe_some_string.map(|s| s.len());
  ///
  /// assert_eq!(maybe_some_len, Some(13));
  /// <code>
  /// </summary>
  /// <param name="f"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public Option<U> Map<U>(Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the provided default result (if none),
  /// or applies a function to the contained value (if any).
  ///
  /// Arguments passed to <c>map_or<c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>map_or_else</c>,
  /// which is lazily evaluated.
  ///
  /// <c>map_or_else</c>: Option::map_or_else
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some("foo");
  /// assert_eq!(x.map_or(42, |v| v.len()), 3);
  ///
  /// let x: Option<string> = None;
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
  /// Computes a default function result (if none), or
  /// applies a different function to the contained value (if any).
  ///
  /// Examples
  ///
  /// <code>
  /// let k = 21;
  ///
  /// let x = Some("foo");
  /// assert_eq!(x.map_or_else(|| 2 * k, |v| v.len()), 3);
  ///
  /// let x: Option<string> = None;
  /// assert_eq!(x.map_or_else(|| 2 * k, |v| v.len()), 42);
  /// <code>
  /// </summary>
  /// <param name="def"></param>
  /// <param name="f"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public U MapOrElse<U>(Func<U> def, Func<T, U> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Transforms the <c>Option<T><c> into a <c>Result<T, E></c>, mapping <c>Some(v)</c> to
  /// <c>Ok(v)</c> and <c>None</c> to <c>Err(err)</c>.
  ///
  /// Arguments passed to <c>ok_or<c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use <c>ok_or_else</c>, which is
  /// lazily evaluated.
  ///
  /// <c>Ok(v)</c>: Ok
  /// <c>Err(err)</c>: Err
  /// <c>Some(v)</c>: Some
  /// <c>ok_or_else</c>: Option::ok_or_else
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some("foo");
  /// assert_eq!(x.ok_or(0), Ok("foo"));
  ///
  /// let x: Option<string> = None;
  /// assert_eq!(x.ok_or(0), Err(0));
  /// <code>
  /// </summary>
  /// <param name="err"></param>
  /// <typeparam name="E"></typeparam>
  /// <returns></returns>
  public Result<T, E> OkOr<E>(E err)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Transforms the <c>Option<T><c> into a <c>Result<T, E></c>, mapping <c>Some(v)</c> to
  /// <c>Ok(v)</c> and <c>None</c> to <c>Err(err())</c>.
  ///
  /// <c>Ok(v)</c>: Ok
  /// <c>Err(err())</c>: Err
  /// <c>Some(v)</c>: Some
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some("foo");
  /// assert_eq!(x.ok_or_else(|| 0), Ok("foo"));
  ///
  /// let x: Option<string> = None;
  /// assert_eq!(x.ok_or_else(|| 0), Err(0));
  /// <code>
  /// </summary>
  /// <param name="err"></param>
  /// <typeparam name="E"></typeparam>
  /// <returns></returns>
  public Result<T, E> OkOrElse<E>(Func<E> err)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the option if it contains a value, otherwise returns <c>optb<c>.
  ///
  /// Arguments passed to <c>or<c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use <c>or_else</c>, which is
  /// lazily evaluated.
  ///
  /// <c>or_else</c>: Option::or_else
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some(2);
  /// let y = None;
  /// assert_eq!(x.or(y), Some(2));
  ///
  /// let x = None;
  /// let y = Some(100);
  /// assert_eq!(x.or(y), Some(100));
  ///
  /// let x = Some(2);
  /// let y = Some(100);
  /// assert_eq!(x.or(y), Some(2));
  ///
  /// let x: Option<int> = None;
  /// let y = None;
  /// assert_eq!(x.or(y), None);
  /// <code>
  /// </summary>
  /// <param name="optB"></param>
  /// <returns></returns>
  public Option<T> Or(Option<T> optB)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the option if it contains a value, otherwise calls <c>f<c> and
  /// returns the result.
  ///
  /// Examples
  ///
  /// <code>
  /// fn nobody() -> Option<&'static str> { None }
  /// fn vikings() -> Option<&'static str> { Some("vikings") }
  ///
  /// assert_eq!(Some("barbarians").or_else(vikings), Some("barbarians"));
  /// assert_eq!(None.or_else(vikings), Some("vikings"));
  /// assert_eq!(None.or_else(nobody), None);
  /// <code>
  /// </summary>
  /// <param name="f"></param>
  /// <returns></returns>
  public Option<T> OrElse(Func<Option<T>> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Replaces the actual value in the option by the value given in parameter,
  /// returning the old value if present,
  /// leaving a <c>Some</c> in its place without deinitializing either one.
  ///
  /// Examples
  ///
  /// <code>
  /// let mut x = Some(2);
  /// let old = x.replace(5);
  /// assert_eq!(x, Some(5));
  /// assert_eq!(old, Some(2));
  ///
  /// let mut x = None;
  /// let old = x.replace(3);
  /// assert_eq!(x, Some(3));
  /// assert_eq!(old, None);
  /// <code>
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public Option<T> Replace(T value)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Takes the value out of the option, leaving a <c>None</c> in its place.
  ///
  /// Examples
  ///
  /// <code>
  /// let mut x = Some(2);
  /// let y = x.take();
  /// assert_eq!(x, None);
  /// assert_eq!(y, Some(2));
  ///
  /// let mut x: Option<int> = None;
  /// let y = x.take();
  /// assert_eq!(x, None);
  /// assert_eq!(y, None);
  /// <code>
  /// </summary>
  /// <returns></returns>
  public Option<T> Take()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Transposes an <c>Option<c> of a <c>Result</c> into a <c>Result</c> of an <c>Option<c>.
  ///
  /// <c>None</c> will be mapped to <code>[Ok]\([None])</code>.
  /// <code>[Some]\([Ok]\(\_))</code> and <code>[Some]\([Err]\(\_))</code> will be mapped to
  /// <code>[Ok]\([Some]\(\_))</code> and <code>[Err]\(\_)</code>.
  ///
  /// Examples
  ///
  /// <code>
  /// #[derive(Debug, Eq, PartialEq)]
  /// struct SomeErr;
  ///
  /// let x: Result<Option<i32>, SomeErr> = Ok(Some(5));
  /// let y: Option<Result<i32, SomeErr>> = Some(Ok(5));
  /// assert_eq!(x, y.transpose());
  /// <code>
  /// </summary>
  /// <typeparam name="E"></typeparam>
  /// <returns></returns>
  public Result<Option<T>, E> Transpose<E>()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Some</c> value, consuming the <c>self<c> value.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the <c>None</c>
  /// case explicitly, or call <c>unwrap_or</c>, <c>unwrap_or_else</c>, or
  /// <c>unwrap_or_default</c>.
  ///
  /// <c>unwrap_or</c>: Option::unwrap_or
  /// <c>unwrap_or_else</c>: Option::unwrap_or_else
  /// <c>unwrap_or_default</c>: Option::unwrap_or_default
  ///
  /// # Panics
  ///
  /// Panics if the self value equals <c>None</c>.
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some("air");
  /// assert_eq!(x.unwrap(), "air");
  /// <code>
  ///
  /// <code>should_panic
  /// let x: Option<string> = None;
  /// assert_eq!(x.unwrap(), "air"); // fails
  /// <code>
  /// </summary>
  /// <returns></returns>
  public T Unwrap()
  {
    if (IsNone())
      throw new PanicException("Attempted to call Option.Unwrap() on a None value.");

    return _value!;
  }

  /// <summary>
  /// Returns the contained <c>Some</c> value or a provided default.
  ///
  /// Arguments passed to <c>unwrap_or<c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>unwrap_or_else</c>,
  /// which is lazily evaluated.
  ///
  /// <c>unwrap_or_else</c>: Option::unwrap_or_else
  ///
  /// Examples
  ///
  /// <code>
  /// assert_eq!(Some("car").unwrap_or("bike"), "car");
  /// assert_eq!(None.unwrap_or("bike"), "bike");
  /// <code>
  /// </summary>
  /// <param name="def"></param>
  /// <returns></returns>
  public T UnwrapOr(T def)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Some</c> value or a default.
  ///
  /// Consumes the <c>self<c> argument then, if <c>Some</c>, returns the contained
  /// value, otherwise if <c>None</c>, returns the [default value] for that
  /// type.
  ///
  /// Examples
  ///
  /// Converts a string to an integer, turning poorly-formed strings
  /// into 0 (the default value for integers). <c>parse</c> converts
  /// a string to any other type that implements <c>FromStr</c>, returning
  /// <c>None</c> on error.
  ///
  /// <code>
  /// let good_year_from_input = "1909";
  /// let bad_year_from_input = "190blarg";
  /// let good_year = good_year_from_input.parse().ok().unwrap_or_default();
  /// let bad_year = bad_year_from_input.parse().ok().unwrap_or_default();
  ///
  /// assert_eq!(1909, good_year);
  /// assert_eq!(0, bad_year);
  /// <code>
  ///
  /// [default value]: Default::default
  /// <c>parse</c>: str::parse
  /// <c>FromStr</c>: crate::str::FromStr
  /// </summary>
  /// <returns></returns>
  public T UnwrapOrDefault()
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns the contained <c>Some</c> value or computes it from a closure.
  ///
  /// Examples
  ///
  /// <code>
  /// let k = 10;
  /// assert_eq!(Some(4).unwrap_or_else(|| 2 * k), 4);
  /// assert_eq!(None.unwrap_or_else(|| 2 * k), 20);
  /// <code>
  /// </summary>
  /// <param name="f"></param>
  /// <returns></returns>
  public T UnwrapOrElse(Func<T> f)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Returns <c>Some</c> if exactly one of <c>self<c>, <c>optb<c> is <c>Some</c>, otherwise returns <c>None</c>.
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some(2);
  /// let y: Option<int> = None;
  /// assert_eq!(x.xor(y), Some(2));
  ///
  /// let x: Option<int> = None;
  /// let y = Some(2);
  /// assert_eq!(x.xor(y), Some(2));
  ///
  /// let x = Some(2);
  /// let y = Some(2);
  /// assert_eq!(x.xor(y), None);
  ///
  /// let x: Option<int> = None;
  /// let y: Option<int> = None;
  /// assert_eq!(x.xor(y), None);
  /// <code>
  /// </summary>
  /// <param name="optB"></param>
  /// <returns></returns>
  public Option<T> Xor(Option<T> optB)
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Zips <c>self<c> with another <c>Option<c>.
  ///
  /// If <c>self<c> is <c>Some(s)<c> and <c>other<c> is <c>Some(o)<c>, this method returns <c>Some((s, o))<c>.
  /// Otherwise, <c>None<c> is returned.
  ///
  /// Examples
  ///
  /// <code>
  /// let x = Some(1);
  /// let y = Some("hi");
  /// let z = None::<u8>;
  ///
  /// assert_eq!(x.zip(y), Some((1, "hi")));
  /// assert_eq!(x.zip(z), None);
  /// <code>
  /// </summary>
  /// <param name="other"></param>
  /// <typeparam name="U"></typeparam>
  /// <returns></returns>
  public Option<(T, U)> Zip<U>(Option<U> other)
  {
    throw new NotImplementedException();
  }
}