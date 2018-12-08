using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Observable
{
	public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
	{
		return new FromCoroutineObservable<T>(coroutine);
	}

	public static IObservable<Unit> FromCoroutine<Unit>(Func<CancellationToken, IEnumerator> coroutine)
	{
		return new FromCoroutineObservable<Unit>((observer, cancellationToken) => CoroutineWrap(observer, cancellationToken, coroutine));
	}

	private static IEnumerator CoroutineWrap<Unit>(IObserver<Unit> observer, CancellationToken cancellationToken, Func<CancellationToken,IEnumerator> originCoroutine)
	{
		var c = originCoroutine(cancellationToken);

		while (c.MoveNext())
		{
			observer.OnNext(default(Unit));
			
			yield return null;
		}
		
		observer.OnComplete();
	}
}
