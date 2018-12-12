using System.Collections;
using System.Collections.Generic;
using System;

public partial class Observable {

	public static IObservable<TR> FromSubThread<T, TR>(Func<T, CancellationToken, TR> execFunc, T inputVal = default(T))
	{
		return new FromSubThread<T,TR>(execFunc, inputVal);
	}

	public static IObservable<TR> ContinueWithFuncInSubThread<T, TR>(this IObservable<T> source, Func<T, CancellationToken, TR> execFunc)
	{
		//TODO 去掉lambda函数
		return source.ContinueWith(v => FromSubThread(execFunc, v));
	}
	
}
