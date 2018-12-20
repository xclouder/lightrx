using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightRx
{
    public class CreateObservable<T> : IObservable<T> {
        private readonly Func<IObserver<T>, IDisposable> _subscribe;

        public CreateObservable(Func<IObserver<T>, IDisposable> subscribe)
        {
            _subscribe = subscribe;
        }
            
        public IDisposable Subscribe(IObserver<T> observer)
        {
            var cancel = new SingleAssignmentDisposable();
            observer = new InnerCreateObserver<T>(observer, cancel);

            var disposable = _subscribe(observer) ?? Disposable.Empty;
            cancel.Disposable = disposable;

            return cancel;
        }
        
    }
    
    public class CreateObservable<TState, T> : IObservable<T>
    {

        private readonly TState _state;
        private readonly Func<TState, IObserver<T>, IDisposable> _subscribe;

        public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
        {
            _state = state;
            _subscribe = subscribe;
        }
            
        public IDisposable Subscribe(IObserver<T> observer)
        {
            var cancel = new SingleAssignmentDisposable();
            observer = new InnerCreateObserver<T>(observer, cancel);

            var disposable = _subscribe(_state, observer) ?? Disposable.Empty;
            cancel.Disposable = disposable;

            return cancel;
        }

    }
    
    internal class InnerCreateObserver<T> : OperatorObserverBase<T, T>
    {
        public InnerCreateObserver(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
        {
        }

        public override void OnNext(T value)
        {
            Observer.OnNext(value);
        }

        public override void OnError(Exception error)
        {
            try { Observer.OnError(error); }
            finally { Dispose(); }
        }

        public override void OnComplete()
        {
            try { Observer.OnComplete(); }
            finally { Dispose(); }
        }
    }
    
}
