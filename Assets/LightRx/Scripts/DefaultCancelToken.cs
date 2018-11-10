using System;


    public class DefaultCancelToken : IDisposable
    {
        public IDisposable SourceDisposable { get; set; }
        
        public void Dispose()
        {
            if (SourceDisposable != null)
            {
                SourceDisposable.Dispose();
            }
        }
    }

    public static class Disposable
    {
        public static IDisposable CreateDefault(IDisposable src)
        {
            var token = new DefaultCancelToken();
            token.SourceDisposable = src;

            return token;
        }
    }
