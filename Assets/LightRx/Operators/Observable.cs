using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightRx
{

    public static partial class Observable {

        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
        {
            return new CreateObservable<T>(subscribe);
        }
        
        public static IObservable<T> Create<TState, T>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
        {
            return new CreateObservable<TState,T>(state, subscribe);
        }
        
        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> whereFunc)
        {
            return new WhereObservable<T>(source, whereFunc);
        }

        public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selectFunc)
        {
            return new SelectObservable<T,TR>(source, selectFunc);
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> subscribeAction, Action completionAction = null, Action<Exception> errorAction = null)
        {
            return source.Subscribe(new ActionObserver<T>(subscribeAction, completionAction, errorAction));
        }

        public static IObservable<T[]> WhenAll<T>(params IObservable<T>[] observables)
        {
            return new WhenAllObservable<T>(observables);
        }

        public static IObservable<TR> ContinueWith<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
        {
            return new ContinueWithObservable<T,TR>(source, selector);
        }

        public static IObservable<T> Race<T>(RaceMode mode, params IObservable<T>[] observables)
        {
            return new RaceObservable<T>(mode, observables);
        }
	
        public static IObservable<T> Race<T>(params IObservable<T>[] observables)
        {
            return Race(RaceMode.CompleteOrError, observables);
        }
    }

}

