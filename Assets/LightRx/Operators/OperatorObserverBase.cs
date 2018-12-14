using System;

namespace LightRx
{
    public abstract class OperatorObserverBase<T, TR> : IObserver<T>, IDisposable
    {

        protected IObserver<TR> Observer;
        protected IDisposable _cancel;
	
        public OperatorObserverBase(IObserver<TR> observer, IDisposable cancel)
        {
            Observer = observer;
            _cancel = cancel;
        }

        public abstract void OnNext(T value);

        public abstract void OnComplete();

        public abstract void OnError(Exception error);
	
        public void Dispose()
        {
            _cancel.Dispose();
        }
    }


}
