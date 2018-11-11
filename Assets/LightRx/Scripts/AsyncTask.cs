using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncTask<T, TR> : IObservable<T> {
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		throw new NotImplementedException();
	}
	
}
