

using System;

public static class Observable
{
    
    public static IObservable<TResult> Find<TInput, TResult>(this IObservable<TInput> src, System.Func<TInput, TResult> func = null)
    {
        return null;
    }
    
    public static IObservable<T> Where<T>(this IObservable<T> src, Func<T, bool> predicate)
    {
        return new Where<T>(src, predicate);
    }

    public static IObservable<TR> Select<T, TR>(this IObservable<T> src, Func<T, TR> selectFunc)
    {
        return new Select<T,TR>(src, selectFunc);
    }
    
    
}