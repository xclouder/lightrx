using System;

namespace LightRx
{
    public struct CancellationToken
    {
        readonly ICancelable source;

        public static readonly CancellationToken Empty = new CancellationToken(null);

        public CancellationToken(ICancelable source)
        {
            this.source = source;
        }

        public bool IsCancellationRequested
        {
            get
            {
                return source != null && source.IsCanceled;
            }
        }

        public void ThrowIfCancellationRequested()
        {
            if (IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
        }
    }

}
