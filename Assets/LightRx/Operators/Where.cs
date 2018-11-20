using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Where<T> : IObservable<T>
{

	private readonly IObservable<T> _source;
	private Func<T, bool> _whereFunc;
	
	public Where(IObservable<T> source, Func<T, bool> whereFunc)
	{
		_source = source;
		_whereFunc = whereFunc;
	}
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		var disposable = _source.Subscribe(new InnerWhereObserver(this, observer));
		var d = new SingleAssignmentDisposable(disposable);
		
		return d;
	}
	
	private class InnerWhereObserver : IObserver<T>
	{
		private readonly IObserver<T> _observer;
		private readonly Where<T> _parent;

		public InnerWhereObserver(Where<T> parent, IObserver<T> observer)
		{
			_parent = parent;
			_observer = observer;
		}
		
		public void OnNext(T value)
		{
			if (_parent._whereFunc(value))
			{
				_observer.OnNext(value);	
			}
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
}
