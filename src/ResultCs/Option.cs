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
  // ✓       ✓   ✓    T GetOrInsertWith(Func<T> f)
  // ✓       ✓   ✓    T Insert(T value)
  // ✓       ✓   ✓    bool IsNone()
  // ✓       ✓   ✓    bool IsSome()
  // ✓       ✓   ✓    bool IsSomeAnd(Func<T, bool> f)
  // ✓       ✓   ✓    IEnumerable<Option<T>> Iter()
  // ✓       ✓   ✓    Option<U> Map<U>(Func<T, U> f)
  // ✓       ✓   ✓    U MapOr<U>(U def, Func<T, U> f)
  // ✓       ✓   ✓    U MapOrElse<U>(Func<U> def, Func<T, U> f)
  // ✓       ✓   ✓    Result<T, E> OkOr<E>(E err)
  // ✓       ✓   ✓    Result<T, E> OkOrElse<E>(Func<E> err)
  // ✓       ✓   ✓    Option<T> Or(Option<T> optB)
  // ✓       ✓   ✓    Option<T> OrElse(Func<Option<T>> f)
  // ✓       ✓   ✓    Option<T> Replace(T value)
  // ✓       ✓   ✓    Option<T> Take()
  // ✓       ✓   ✓    Result<Option<T>, E> Transpose<E>()
  // ✓       ✓   ✓    T Unwrap()
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
    if (other is null) return false;

    var otherOption = (Option<T>)other;

    if (this.Kind != otherOption.Kind)
      return false;

    if (this.IsNone() && otherOption.IsNone())
      return true;

    if (_value is null)
    {
      if (otherOption.Unwrap() is null)
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
  public static bool operator ==(Option<T> opt1, Option<T> opt2)
  {
    if (opt1 is null)
    {
      if (opt2 is null)
        return true;

      return false;
    }

    return opt1.Equals(opt2);
  }

  /// <summary>
  /// Convenience override of the <c>!=</c> operator.
  /// </summary>
  /// <param name="opt1">The first <c>Option</c> to compare</param>
  /// <param name="opt2">The second <c>Option</c> to compare</param>
  /// <returns>The result of <c>!opt1.Equals(opt2)</c></returns>
  public static bool operator !=(Option<T> opt1, Option<T> opt2) => !(opt1 == opt2);

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

    if (predicate.Invoke(this.Unwrap()))
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
    if (this.IsNone() && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Option<>))
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

    if (this.IsNone())
    {
      this._value = value;
      this.Kind = OptionKind.Some;
    }

    return _value!;
  }

  /// <summary>
  /// Inserts a value computed from <c>f<c> into the option if it is <c>None</c> and returns
  /// the value stored.
  /// 
  /// Note: This differs from the original implementation because it doesn't
  /// return a mutable reference to the contained value due to differences in
  /// how the languages work.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.None();
  /// var y = x.GetOrInsertWith(() => 5);
  /// 
  /// Assert.Equal(5, y);
  /// Assert.Equal(Option<int>.Some(5), x);
  /// <code>
  /// </summary>
  /// <param name="f">The function to use to populate <c>this</c> if it's currently <c>None</c>.</param>
  /// <returns>The value contained in <c>this</c>.</returns>
  public T GetOrInsertWith(Func<T> f)
  {
    ArgumentNullException.ThrowIfNull(f);

    if (this.IsNone())
    {
      var value = f.Invoke();

      if (value is null)
        throw new PanicException($"Function result returned null - cannot assign null to an Option<{typeof(T).ToString()}>.");

      this._value = value;
      this.Kind = OptionKind.Some;
    }

    return _value!;
  }

  /// <summary>
  /// Inserts <c>value<c> into the option, then returns the value stored.
  ///
  /// If the option already contains a value, the old value is dropped.
  ///
  /// See also <c>Option.GetOrInsert</c>, which doesn't update the value if
  /// the option already contains <c>Some</c>.
  ///
  /// Note: This differs from the original implementation because it doesn't
  /// return a mutable reference to the contained value due to differences in
  /// how the languages work.
  /// 
  /// # Example
  ///
  /// <code>
  /// var opt = Option<int>.None();
  /// var val = opt.Insert(1);
  /// Assert.Equal(1, val);
  /// Assert.Equal(1, opt.Unwrap());
  /// 
  /// val = opt.Insert(2);
  /// Assert.Equal(2, val);
  /// Assert.Equal(Option<int>.Some(2), opt);
  /// <code>
  /// </summary>
  /// <param name="value">The value to insert into <c>this</c>, 
  /// replacing any existing value.</param>
  /// <returns>The new value contained in <c>this</c>.</returns>
  public T Insert(T value)
  {
    ArgumentNullException.ThrowIfNull(value);

    _value = value;
    this.Kind = OptionKind.Some;

    return _value;
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
  /// var x = Option<int>.Some(2);
  /// Assert.True(x.IsSomeAnd(x => x > 1));
  ///
  /// var x = Option<int>.Some(0);
  /// Assert.False(x.IsSomeAnd(x => x > 1));
  ///
  /// var x = Option<int>.None();
  /// Assert.False(x.IsSomeAnd(x => x > 1));
  /// <code>
  /// </summary>
  /// <param name="f">The function used to evaluate the value of <c>this</c>
  /// if it's <c>Some</c>.</param>
  /// <returns><c>true</c> if:
  /// <ul>
  /// <li><c>this</c> is <c>Some</c> AND</li>
  /// <li><c>f</c> returns <c>true</c></li>
  /// </ul>
  /// <c>false</c> otherwise.</returns>
  public bool IsSomeAnd(Func<T, bool> f)
  {
    if (this.IsNone())
      return false;

    return f.Invoke(this.Unwrap());
  }

  /// <summary>
  /// Returns an iterator over the possibly contained value.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.Some(4);
  /// var iter = x.Iter().GetEnumerator();
  /// 
  /// Assert.True(iter.MoveNext());
  /// Assert.Equal(Option<int>.Some(4), iter.Current);
  /// Assert.False(iter.MoveNext());
  ///
  /// var x = Option<int>.None();
  /// var iter = x.Iter().GetEnumerator();
  ///
  /// Assert.True(iter.MoveNext());
  /// Assert.Equal(Option<int>.None(), iter.Current);
  /// Assert.False(iter.MoveNext());
  /// <code>
  /// </summary>
  /// <returns>An <c>IEnumerable<Option<T>></c> containing a single value. 
  /// <ul>
  /// <li>If <c>this</c> is <c>OptionKind.Some</c>, the value is an <c>OptionKind.Some</c> with the unwrapped value of <c>this</c>
  /// <li>If <c>this</c> is <c>OptionKind.None</c>, the value is an <c>OptionKind.None</c>
  /// </ul></returns>
  public IEnumerable<Option<T>> Iter()
  {
    if (this.IsSome())
      return new List<Option<T>> { Option<T>.Some(this.Unwrap()) };

    return new List<Option<T>> { Option<T>.None() };
  }

  /// <summary>
  /// Maps an <c>Option<T><c> to <c>Option<U><c> by applying a function to a contained value.
  ///
  /// Examples
  ///
  /// Converts an <code>Option<string></code> into an <code>Option<int></code>, consuming
  /// the original:
  ///
  /// <code>
  /// var maybeSomeString = Option<string>.Some("Hello, World!");
  /// 
  /// var maybeSomeLen = maybeSomeString.Map(s => s.Length);
  ///
  /// Assert.Equal(Option<int>.Some(13), maybeSomeLen);
  /// <code>
  /// </summary>
  /// <param name="f">The function to be applied to the value if <c>this</c> is <c>OptionKind.Some</c>.</param>
  /// <typeparam name="U">The return type of <c>f</c>.s</typeparam>
  /// <returns>If <c>this</c> is <c>OptionKind.Some</c>, an <c>Option</c> containing the output of <c>f</c> 
  /// applied to <c>this</c>; an <c>OptionKind.None</c> otherwise.</returns>
  public Option<U> Map<U>(Func<T, U> f)
  {
    if (this.IsNone())
      return Option<U>.None();

    if (f is null)
      throw new PanicException("Cannot call Option.Map() with a null delegate.");

    var result = f.Invoke(this.Unwrap());

    if (result is null)
      throw new PanicException("Output of Options.Map() delegate function cannot be null.");

    return Option<U>.Some(result);
  }

  /// <summary>
  /// Returns the provided default result (if none),
  /// or applies a function to the contained value (if any).
  ///
  /// Arguments passed to <c>MapOr<c> are eagerly evaluated; if you are passing
  /// the result of a function call, it is recommended to use <c>MapOrElse</c>,
  /// which is lazily evaluated.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<string>.Some("foo");
  /// Assert.Equal(3, x.MapOr(42, v => v.Length));
  ///
  /// var x = Option<string>.None();
  /// Assert.Equal(42, x.MapOr(42, v => v.Length));
  /// <code>
  /// </summary>
  /// <param name="def">The default value to return if <c>this</c> is <c>OptionKind.None</c>.</param>
  /// <param name="f">The function to be applied to the value if <c>this</c> is <c>OptionKind.Some</c>.</param>
  /// <typeparam name="U">The output type of <c>f</c>.</typeparam>
  /// <returns>If <c>this</c> is <c>OptionKind.Some</c>, an <c>Option</c> that's the output of applying 
  /// <c>f</op> to the current <c>Options</c>'s <c>Some</c> value; otherwise <c>def</c>.</returns>
  public U MapOr<U>(U def, Func<T, U> f)
  {
    if (this.IsNone())
    {
      if (def is null)
        throw new PanicException("The default return value for Option.MapOr() cannot be null.");

      return def;
    }

    if (f is null)
      throw new PanicException("Cannot call Option.MapOr() with a null delegate.");

    var result = f.Invoke(this.Unwrap());

    if (result is null)
      throw new PanicException("Output of Options.MapOr() delegate function cannot be null.");

    return result;
  }

  /// <summary>
  /// Computes a default function result (if none), or
  /// applies a different function to the contained value (if any).
  ///
  /// Examples
  ///
  /// <code>
  /// const int k = 21;
  ///
  /// var x = Option<string>.Some("foo");
  /// Assert.Equal(3, x.MapOrElse(() => 2 * k, v => v.Length));
  ///
  /// var x = Option<string>.None();
  /// Assert.Equal(42, x.MapOrElse(() => 2 * k, v => v.Length));
  /// <code>
  /// </summary>
  /// <param name="def">The default function to invoke
  /// if <c>this</c> is <c>OptionKind.None</c>.</param>
  /// <param name="f">The function to be applied to the value if <c>this</c> is <c>OptionKind.Some</c>.</param>
  /// <typeparam name="U">The output type of <c>def</c> and <c>f</c>.</typeparam>
  /// <returns>If <c>this</c> is <c>OptionKind.Some</c>, a value that's the output of applying 
  /// <c>f</op> to the current <c>Options</c>'s <c>Some</c> value; otherwise the value of 
  /// invoking <c>def</c>.</returns>
  public U MapOrElse<U>(Func<U> def, Func<T, U> f)
  {
    if (this.IsNone())
    {
      if (def is null)
        throw new PanicException("The default delegate for Option.MapOrElse() cannot be null.");

      var noneResult = def.Invoke();

      if (noneResult is null)
        throw new PanicException("The default delegate for Option.MapOrElse() cannot return null.");

      return noneResult;
    }

    if (f is null)
      throw new PanicException("Cannot call Option.MapOrElse() with a null delegate.");

    var someResult = f.Invoke(this.Unwrap());

    if (someResult is null)
      throw new PanicException("Output of Options.MapOrElse() delegate function cannot be null.");

    return someResult;
  }

  /// <summary>
  /// Transforms the <c>Option<T><c> into a <c>Result<T, E></c>, mapping <c>Some(v)</c> to
  /// <c>Ok(v)</c> and <c>None</c> to <c>Err(err)</c>.
  ///
  /// Arguments passed to <c>OkOr<c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use <c>OkOrElse</c>, which is
  /// lazily evaluated.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<string>.Some("foo");
  /// Assert.Equal(Result<string, int>.Ok("foo"), x.OkOr(0));
  ///
  /// var x = Option<string>.None();
  /// Assert.Equal(Result<string, int>.Err(0), x.OkOr(0));
  /// <code>
  /// </summary>
  /// <param name="err">The error used for the <c>Result</c> if <c>this</c> is <c>OptionKind.None</c>.</param>
  /// <typeparam name="E">The type of the <c>Result</c>'s <c>ResultKind.Err</c>.</typeparam>
  /// <returns>If <c>this</c> is <c>OptionKind.Some</c>, a <c>Result</c> with <c>ResultKind.Ok</c> with the
  /// <c>Some</c> value of <c>this</c>; otherwise a <c>Result</c> with <c>ResultKind.Err</c> with the
  /// value of <c>err</c>.</returns>
  public Result<T, E> OkOr<E>(E err)
  {
    if (this.IsNone())
    {
      if (err is null)
        throw new PanicException("Option.OkOr() cannot be passed a null error value.");

      return Result<T, E>.Err(err);
    }

    return Result<T, E>.Ok(this.Unwrap());
  }

  /// <summary>
  /// Transforms the <c>Option<T><c> into a <c>Result<T, E></c>, mapping <c>Some(v)</c> to
  /// <c>Ok(v)</c> and <c>None</c> to <c>Err(err)</c>.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<string>.Some("foo");
  /// Assert.Equal(Result<string, int>.Ok("foo"), x.OkOrElse(() => 0));
  ///
  /// var x = Option<string>.None();
  /// Assert.Equal(Result<string, int>.Err(0), x.OkOrElse(() => 0));
  /// <code>
  /// </summary>
  /// <param name="err">The function to call to get the <c>Err</c> value for the <c>Result</c> 
  /// if <c>this</c> is <c>OptionKind.None</c>.</param>
  /// <typeparam name="E">The type of the <c>Result</c>'s <c>ResultKind.Err</c>.</typeparam>
  /// <returns>If <c>this</c> is <c>OptionKind.Some</c>, a <c>Result</c> with <c>ResultKind.Ok</c> with the
  /// <c>Some</c> value of <c>this</c>; otherwise a <c>Result</c> with <c>ResultKind.Err</c> with the
  /// value resulting from calling <c>err</c>.</returns>
  public Result<T, E> OkOrElse<E>(Func<E> err)
  {
    if (this.IsNone())
    {
      if (err is null)
        throw new PanicException("Option.OkOrElse() cannot be passed a null error delegate.");

      var errValue = err.Invoke();

      if (errValue is null)
        throw new PanicException("The error delegate for Option.OkOrElse() cannot return a null value.");

      return Result<T, E>.Err(errValue);
    }

    return Result<T, E>.Ok(this.Unwrap());
  }

  /// <summary>
  /// Returns the option if it contains a value, otherwise returns <c>optB<c>.
  ///
  /// Arguments passed to <c>Or<c> are eagerly evaluated; if you are passing the
  /// result of a function call, it is recommended to use <c>OrElse</c>, which is
  /// lazily evaluated.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.Some(2);
  /// var y = Option<int>.None();
  /// Assert.Equal(Option<int>.Some(2), x.Or(y));
  ///
  /// var x = Option<int>.None();
  /// var y = Option<int>.Some(100);
  /// Assert.Equal(Option<int>.Some(100), x.Or(y));
  ///
  /// var x = Option<int>.Some(2);
  /// var y = Option<int>.Some(100);
  /// Assert.Equal(Option<int>.Some(2), x.Or(y));
  ///
  /// var x = Option<int>.None();
  /// var y = Option<int>.None();
  /// Assert.Equal(Option<int>.None(), x.Or(y));
  /// <code>
  /// </summary>
  /// <param name="optB">The value to return if <c>this</c> is <c>OptionKind.None</c></param>
  /// <returns><c>this</c> if <c>this</c> if <c>OptionKind.Some</c>, <c>optB</c> otherwise.</returns>
  public Option<T> Or(Option<T> optB)
  {
    if (this.IsNone())
    {
      if (optB is null)
        throw new PanicException("Cannot pass a null alternative value to Option.Or().");

      return optB;
    }

    return Option<T>.Some(this.Unwrap());
  }

  /// <summary>
  /// Returns the option if it contains a value, otherwise calls <c>f<c> and
  /// returns the result.
  ///
  /// Examples
  ///
  /// <code>
  /// public Option<string> Nobody() => Option<string>.None();
  /// public Option<string> Vikings() => Option<string>.Some("vikings");
  ///
  /// Assert.Equal(Option<string>.Some("barbarians"), Option<string>.Some("barbarians").OrElse(Vikings));
  /// Assert.Equal(Option<string>.Some("vikings"), Option<string>.None().OrElse(Vikings));
  /// Assert.Equal(Option<string>.None(), Option<string>.None().OrElse(Nobody));
  /// <code>
  /// </summary>
  /// <param name="f">The function to invoke when <c>this</c> is <c>Some</c>.</param>
  /// <returns>The result of <c>f</c> if <c>this</c> is <c>None</c>; otherwise <c>self<c>.</returns>
  public Option<T> OrElse(Func<Option<T>> f)
  {
    if (this.IsNone())
    {
      if (f is null)
        throw new PanicException("Cannot pass a null delegate function to Option.OrElse().");

      var noneValue = f.Invoke();

      if (noneValue is null)
        throw new PanicException("The delegate function of Option.OrElse(0 cannot return null.)");

      return noneValue;
    }

    return Option<T>.Some(this.Unwrap());
  }

  /// <summary>
  /// Replaces the actual value in the option by the value given in parameter,
  /// returning the old value if present,
  /// leaving a <c>Some</c> in its place without deinitializing either one.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.Some(2);
  /// var old = x.Replace(5);
  /// Assert.Equal(Option<int>.Some(5), x);
  /// Assert.Equal(Option<int>.Some(2), old);
  ///
  /// var x = Option<int>.None();
  /// var old = x.Replace(3);
  /// Assert.Equal(Option<int>.Some(3), x);
  /// Assert.Equal(Option<int>.None(), old);
  /// <code>
  /// </summary>
  /// <param name="value">The value to upsert into this <c>Option</c>.</param>
  /// <returns>If <c>this</c> is <c>Some</c>, an <c>Option</c> with the value that is replaced;
  /// otherwise and <c>Option.None</c>.</returns>
  public Option<T> Replace(T value)
  {
    if (value is null)
      throw new PanicException("Cannot pass a null value to Option.Replace().");

    Option<T> oldOpt;

    if (this.IsNone())
    {
      oldOpt = Option<T>.None();
      this.Kind = OptionKind.Some;
    }
    else
      oldOpt = Option<T>.Some(this.Unwrap());

    _value = value;

    return oldOpt;
  }

  /// <summary>
  /// Takes the value out of the option, leaving a <c>None</c> in its place.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<int>.Some(2);
  /// var y = x.Take();
  /// Assert.Equal(Option<int>.None(), x);
  /// Assert.Equal(Option<int>.Some(2), y);
  ///
  /// var x = Option<int>.None();
  /// var y = x.Take();
  /// Assert.Equal(Option<int>.None(), x);
  /// Assert.Equal(Option<int>.None(), y);
  /// <code>
  /// </summary>
  /// <returns>If <c>this</c> is <c>Some</c>, an <c>Option</c> that wraps the previous
  /// value; otherwise <c>Option.None</c>.</returns>
  public Option<T> Take()
  {
    if (this.IsNone())
      return Option<T>.None();

    var oldOpt = Option<T>.Some(this.Unwrap());

    this.Kind = OptionKind.None;
    this._value = default(T);

    return oldOpt;
  }

  /// <summary>
  /// Transposes an <c>Option<c> of a <c>Result</c> into a <c>Result</c> of an <c>Option<c>.
  ///
  /// <c>None</c> will be mapped to <c>Ok(None)</c>.
  /// <c>Some(Ok)</c> and <c>Some(Err)</c> will be mapped to
  /// <c>Ok(Some)</c> and <c>Err</code>.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Result<Option<int>, string>.Ok(Option<int>.Some(5));
  /// var y = Option<Result<int, string>>.Some(Result<int, string>.Ok(5));
  /// Assert.Equal(x, y.Transpose<int, string>());
  /// <code>
  /// </summary>
  /// <typeparam name="U">The internal type of the <c>Result<Option<>></c>. (Note: this is a 
  /// deviation from the Rust signature because C# doesn't implement enum values the way 
  /// Rust does.)</typeparam>
  /// <typeparam name="E">The type of the error for the <c>Result</c>.</typeparam>
  /// <returns>An <c>Option<c> of a <c>Result</c> as a <c>Result</c> of an <c>Option<c></returns>
  public Result<Option<U>, E> Transpose<U, E>()
  {
     if (!typeof(T).IsGenericType || typeof(T).GetGenericTypeDefinition() != typeof(Result<,>))
        throw new PanicException($"The wrapped type is {_value!.GetType()}; expecting Result<{typeof(U)}, {typeof(E)}>");

    if (this.IsSome())
    {
      var okType = typeof(T).GetGenericArguments()[0];

      if (typeof(U) != okType)
        throw new PanicException($"The wrapped value type {okType} not assignable to type parameter {typeof(U)}");

      var errType = typeof(T).GetGenericArguments()[1];

      if (typeof(E) != errType)
        throw new PanicException($"The wrapped error type {errType} not assignable to type parameter {typeof(E)}");

      Result<U, E>? result = this.Unwrap() as Result<U, E>;

      if(result!.IsErr())
        return Result<Option<U>, E>.Err(result.UnwrapErr());

      return Result<Option<U>, E>.Ok(Option<U>.Some(result.Unwrap()));
    }
  
    return Result<Option<U>, E>.Ok(Option<U>.None());
  }

  /// <summary>
  /// Returns the contained <c>Some</c> value, consuming the <c>self<c> value.
  ///
  /// Because this function may panic, its use is generally discouraged.
  /// Instead, prefer to use pattern matching and handle the <c>None</c>
  /// case explicitly, or call <c>UnwrapOr</c>, <c>UnwrapOrElse</c>, or
  /// <c>UnwrapOrDefault</c>.
  ///
  /// Throws <exception cref="PanicException"/> if the self value equals <c>None</c>.
  ///
  /// Examples
  ///
  /// <code>
  /// var x = Option<string>.Some("air");
  /// Assert.Equal("air". x.Unwrap());
  /// <code>
  ///
  /// <code>
  /// var x = Option<string>.None();
  /// Assert.Throws<PanicException>(() => x.unwrap());
  /// <code>
  /// </summary>
  /// <returns>The contained <c>Some</c> value, consuming the <c>self<c> value.</returns>
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