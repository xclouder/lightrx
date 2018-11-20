using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Observable {

	public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> whereFunc)
	{
		return null;
	}

	public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selectFunc)
	{
		return null;
	}

	public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> subscribeAction)
	{
		return null;
	}
	
}
