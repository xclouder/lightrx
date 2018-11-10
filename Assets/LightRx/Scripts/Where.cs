using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Where<T> : IObservable<T>
{

	private IObservable<T> _src;
	public Func<T, bool> _predicate;
	
	public Where(IObservable<T> src, Func<T, bool> predicate)
	{
		_src = src;
		_predicate = predicate;
	}
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		var srcDisposable = _src.Subscribe(new Where_InnerObserver<T>(observer, this));
		var disposable = Disposable.CreateDefault(srcDisposable);

		return disposable;
	}

	
}

internal class Where_InnerObserver<T> : IObserver<T>
{
	private IObserver<T> _target;
	private Where<T> _parent;
	
	public Where_InnerObserver(IObserver<T> target, Where<T> parent)
	{
		_target = target;
		_parent = parent;
	}
	
	public void OnNotify(T value)
	{
		if (_parent._predicate(value))
		{
			_target.OnNotify(value);
		}
	}

	public void OnError(Exception error)
	{
		throw new NotImplementedException();
	}
}
