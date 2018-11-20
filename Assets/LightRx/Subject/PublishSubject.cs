using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublishSubject<T> : IObservable<T>, IObserver<T>
{
	private readonly ListObserver<T> _observers = new ListObserver<T>();
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		var subscription = new Subscription(this, observer);
		_observers.AddObserver(observer);
		
		return subscription;
	}

	public void OnNext(T value)
	{
		_observers.OnNext(value);
	}

	public void OnComplete()
	{
		_observers.OnComplete();
	}

	public void OnError(Exception error)
	{
		_observers.OnError(error);
	}

	private class Subscription : IDisposable
	{
		private PublishSubject<T> _parent;
		private IObserver<T> _observer;
		
		public Subscription(PublishSubject<T> parent, IObserver<T> observer)
		{
			_parent = parent;
			_observer = observer;
		}
		
		public void Dispose()
		{
			_parent._observers.RemoveObserver(_observer);
		}
	}
	
}
