using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueWithObservable<T, TR> : IObservable<TR>
{

	private readonly IObservable<T> _source;
	private Func<T, IObservable<TR>> _selector;
	
	public ContinueWithObservable(IObservable<T> source, Func<T, IObservable<TR>> selector)
	{
		_source = source;
		_selector = selector;
	}
	
	public IDisposable Subscribe(IObserver<TR> observer)
	{
		var cancel = new SingleAssignmentDisposable();

		//TODO
		var disposable = _source.Subscribe(new InnerContinueWithObserver(this, observer, cancel));
		cancel.Disposable = disposable;
		
		return cancel;
	}

	private class InnerContinueWithObserver : OperatorObserverBase<T, TR>
	{
		private readonly ContinueWithObservable<T, TR> _parent;
		private bool _seenValue = false;
		private T _lastValue;
		
		private SerialDisposable _serialDisposable = new SerialDisposable();
		
		public InnerContinueWithObserver(ContinueWithObservable<T, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
		{
			_parent = parent;
		}

		public IDisposable Run()
		{
			var sourceDisposable = new SingleAssignmentDisposable();
			_serialDisposable.Disposable = sourceDisposable;

			sourceDisposable.Disposable = _parent._source.Subscribe(this);
			return _serialDisposable;
		}
		
		public override void OnNext(T value)
		{
			_seenValue = true;
			_lastValue = value;
		}

		public override void OnComplete()
		{
			if (_seenValue)
			{
				var v = _parent._selector(_lastValue);
				// dispose source subscription
				_serialDisposable.Disposable = v.Subscribe(Observer);
			}
			else
			{
				try { Observer.OnComplete(); } finally { Dispose(); };
			}
		}

		public override void OnError(Exception error)
		{
			try { Observer.OnError(error); } finally { Dispose(); };
		}
	}
	
}
