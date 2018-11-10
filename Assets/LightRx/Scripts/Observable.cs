

public static class Observable
{
    
    public static IObservable<TResult> Find<TInput, TResult>(this IObservable<TInput> src, System.Func<TInput, TResult> func = null)
    {
        return null;
    }
    
    public static IObservable<T> Where<T>(this IObservable<T> src, System.Func<T, bool> predicate)
    {
        return new Where<T>(src, predicate);
    }

    public static IObservable<T> Filter<T>(this IObservable<T> src)
    {
        return null;
    }
    
    
}