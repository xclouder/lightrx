using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Select<TSource, TTarget> : IObservable<TTarget>
{

	private IObservable<TSource> _src;
	private Func<TSource, TTarget> _selectFunc;

	public Select(IObservable<TSource> src, Func<TSource, TTarget> selectFunc)
	{
		_src = src;
		_selectFunc = selectFunc;
	}
	
	public IDisposable Subscribe(IObserver<TTarget> observer)
	{

		var srcDisposable = _src.Subscribe(new InnerSelectObserver<TSource, TTarget>(this, observer));

		var disposable = Disposable.CreateDefault(srcDisposable);
		return disposable;
		
	}

	private class InnerSelectObserver<TInput, TResult> : IObserver<TInput>
	{
		private Select<TInput, TResult> _parent;
		private IObserver<TResult> _observer;
		public InnerSelectObserver(Select<TInput, TResult> parent, IObserver<TResult> observer)
		{
			_parent = parent;
			_observer = observer;
		}
		
		public void OnNotify(TInput value)
		{
			var result = _parent._selectFunc(value);
			
			_observer.OnNotify(result);
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
	
}
