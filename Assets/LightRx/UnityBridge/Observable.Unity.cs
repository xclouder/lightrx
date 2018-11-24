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

}
