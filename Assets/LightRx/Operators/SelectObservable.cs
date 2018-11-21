using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObservable<T, TR> : IObservable<TR>
{

	private readonly IObservable<T> _source;
	private readonly Func<T, TR> _selectFunc;
	
	public SelectObservable(IObservable<T> source, Func<T, TR> selectFunc)
	{
		_source = source;
		_selectFunc = selectFunc;
	}
	
	public IDisposable Subscribe(IObserver<TR> observer)
	{
		var disposable = _source.Subscribe(new InnerSelectObserver(this, observer));
		var d = new SingleAssignmentDisposable(disposable);

		return d;
	}


	private class InnerSelectObserver : IObserver<T>
	{
		private readonly SelectObservable<T, TR> _parent;
		private readonly IObserver<TR> _observer;

		public InnerSelectObserver(SelectObservable<T, TR> parent, IObserver<TR> observer)
		{
			_parent = parent;
			_observer = observer;
		}
		
		public void OnNext(T value)
		{
			var retVal = _parent._selectFunc(value);
			_observer.OnNext(retVal);
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
