using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Observable {

	public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> whereFunc)
	{
		return new WhereObservable<T>(source, whereFunc);
	}

	public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selectFunc)
	{
		return new SelectObservable<T,TR>(source, selectFunc);
	}

	public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> subscribeAction)
	{
		return source.Subscribe(new ActionObserver<T>(subscribeAction));
	}
	
}
