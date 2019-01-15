using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Commands;
using UnityEngine;
using Unit = LightRx.Unit;

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

        public delegate void OnActionCompleteDelegate();
        public delegate void DoActionDelegate(OnActionCompleteDelegate onComplete);
        public static IObservable<Unit> FromAtion(DoActionDelegate actionDelegate)
        {
            return Observable.Create<Unit>((observer) =>
            {
                actionDelegate(() =>
                {
                    observer.OnNext(Unit.Default);
                    observer.OnComplete();
                });
                
                //not support cancel
                return Disposable.Empty;
            });
        }

        public static IObservable<Unit> ContinueWithAction(this IObservable<Unit> source, DoActionDelegate actionDelegate)
        {
            return source.ContinueWith<Unit, Unit>((v) => { return Observable.FromAtion(actionDelegate); });
        }

    }

}

