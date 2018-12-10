using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FromSubThread<T, TR> : IObservable<TR>
{
	private IObservable<T> _source;
	
	public FromSubThread(IObservable<T> source)
	{
		_source = source;
	}
	
	public IDisposable Subscribe(IObserver<TR> observer)
	{
		return null;
	}

	private class FromSubThreadInternal : OperatorObserverBase<T, TR>
	{
		public FromSubThreadInternal(IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
		{
		}

		public override void OnNext(T value)
		{
			throw new NotImplementedException();
		}

		public override void OnComplete()
		{
			throw new NotImplementedException();
		}

		public override void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
