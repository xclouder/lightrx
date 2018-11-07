using System;

namespace LightRx
{
    public static class Observerable {

        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            var where = new Where<T>(source, predicate);
            return where;
        }

        public static IObservable<TR> AsyncTask<T, TR>(this IObservable<T> source, Action<T, Action<TR>> asyncTaskExecutor)
        {
            var asyncTask = new AsyncTask<T, TR>(source, asyncTaskExecutor);
            return asyncTask;
        }
    }

}
