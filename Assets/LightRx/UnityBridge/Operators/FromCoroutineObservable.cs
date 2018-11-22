using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromCoroutineObservable<T> : IObservable<T>
{

	private IObservable<T> _source;
	private Func<T, IObserver<T>, CancellationToken, IEnumerator> _coroutine;

	private Coroutine _runningCoroutine;
	
	public FromCoroutineObservable(IObservable<T> source, Func<T, IObserver<T>, CancellationToken, IEnumerator> coroutine)
	{
		_source = source;
		_coroutine = coroutine;
	}
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		return null;
	}

	private class InnerFromCoroutineObserver : IObserver<T>
	{
		private FromCoroutineObservable<T> _parent;
		
		public InnerFromCoroutineObserver(FromCoroutineObservable<T> parent)
		{
			_parent = parent;
		}
		
		public void OnNext(T value)
		{
			throw new NotImplementedException();
		}

		public void OnComplete()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
