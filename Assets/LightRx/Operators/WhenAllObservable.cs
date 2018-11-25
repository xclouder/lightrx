using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhenAllObservable<T> : IObservable<T[]>
{

	private readonly IObservable<T>[] _observables;

	public WhenAllObservable(params IObservable<T>[] observables)
	{
		_observables = observables;
	}
	
	public IDisposable Subscribe(IObserver<T[]> observer)
	{	
		var cancel = new CompositeDisposable();
		var innerObserver = new InnerWhenAllObserver(this, observer, cancel);
		
		foreach (var o in _observables)
		{
			var d = o.Subscribe(innerObserver);
			cancel.Add(d);
		}

		return cancel;
	}

	private class InnerWhenAllObserver : OperatorObserverBase<T, T[]>
	{
		private WhenAllObservable<T> _parent;
		
		public InnerWhenAllObserver(WhenAllObservable<T> parent, IObserver<T[]> observer, IDisposable cancel) : base(observer, cancel)
		{
			_parent = parent;
		}

		public override void OnNext(T value)
		{
			//ignore
		}

		public override void OnComplete()
		{
			
		}

		public override void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
