using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublishSubject<T> : IObservable<T>, IObserver<T>
{
	private IObserver<T> _observer;
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		_observer = observer;
		return null;
	}

	public void OnNext(T value)
	{
		_observer.OnNext(value);
	}

	public void OnComplete()
	{
		_observer.OnComplete();
	}

	public void OnError(Exception error)
	{
		_observer.OnError(error);
	}
}
