public interface Option<T>
{
  //Option<U> And<U>(Option<U> optB);
  //Option<U> AndThen<U>(Func<Option<U>> f);
  //T Expect(string message);
  //Option<T> Filter(Func<T, bool> predicate);
  //Option<T> Flatten();
  
  //For this one, the Kind property needs to be mutable.
  //TODO: Get rid of the subclasses and re-implement in Result.
  //T GetOrInsert();
  T Unwrap();
  OptionKind Kind { get; }
}