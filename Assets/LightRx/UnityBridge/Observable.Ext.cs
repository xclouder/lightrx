using System.Collections;
using System.Collections.Generic;
using System;

public partial class Observable {

	public static IObservable<TR> FromSubThread<T, TR>(Func<T, CancellationToken, TR> execFunc, T inputVal = default(T))
	{
		return new FromSubThread<T,TR>(execFunc, inputVal);
	}
	
}
