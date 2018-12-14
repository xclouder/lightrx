using System;
using System.Collections;
using UnityEngine;

namespace LightRx.Unity
{
    public class FromCoroutineObservable<T> : IObservable<T>
    {
        private readonly Func<IObserver<T>, CancellationToken, IEnumerator> _coroutine;

        private Coroutine _runningCoroutine;
	
        public FromCoroutineObservable(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
        {
            _coroutine = coroutine;
        }
	
        public IDisposable Subscribe(IObserver<T> observer)
        {
            var disposable = new BooleanDisposable();
            var cancellationToken = new CancellationToken(disposable);
		
            var coroutineObserer = new InnerFromCoroutineObserver(observer, disposable);
		
            var c = _coroutine(coroutineObserer, cancellationToken);
		
            MainThreadDispatcher.Instance.StartCoroutine(c);
		
            return disposable;
        }

        private class InnerFromCoroutineObserver : OperatorObserverBase<T,T>
        {
		
            public InnerFromCoroutineObserver(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
            }
		
            public override void OnNext(T value)
            {
                try
                {
                    Observer.OnNext(value);
                }
                catch
                {
                    Dispose();
				
                    throw;
                }
            }

            public override void OnComplete()
            {
                try
                {
                    Observer.OnComplete();
                }
                catch
                {
                    Dispose();
				
                    throw;
                }
			
            }

            public override void OnError(Exception error)
            {
                try
                {
                    Observer.OnError(error);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }
        }
    }

}

