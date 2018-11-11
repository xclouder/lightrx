using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using UnityEngine;



public class ScheduleToSubThread<T> : IObservable<T> {

	private class CancellationToken : ICancelable, IDisposable
	{
		private IDisposable _target;


		public void SetSrcDisposable(IDisposable d)
		{
			_target = d;
		}
		
		private CancelState _state;

		public CancelState State
		{
			get { return _state; }
		}

		public void RequestCancel()
		{
			_state = CancelState.Cancelling;
		}

		public void Dispose()
		{
			RequestCancel();
			_target.Dispose();
		}
	}

	private readonly IObservable<T> _src;
	private Action<T, ICancelable> _action;

	public ScheduleToSubThread(IObservable<T> src, Action<T, ICancelable> action)
	{
		_src = src;
		_action = action;
	}
	
	public IDisposable Subscribe(IObserver<T> observer)
	{
		var cancel = new CancellationToken();
		var srcDisposable = _src.Subscribe(new InnerScheduleToSubThreadObserver<T>(this, observer, cancel));
		cancel.SetSrcDisposable(srcDisposable);

		return cancel;
	}

	private class InnerScheduleToSubThreadObserver<TInput> : IObserver<TInput>
	{
		private IObserver<TInput> _observer;
		private ScheduleToSubThread<TInput> _parent;
		private ICancelable _cancel;
		
		public InnerScheduleToSubThreadObserver(ScheduleToSubThread<TInput> parent, IObserver<TInput> observer, ICancelable cancel)
		{
			_parent = parent;
			_observer = observer;
			_cancel = cancel;
		}
		
		public void OnNotify(TInput value)
		{
			ThreadPool.QueueUserWorkItem(prevVal =>
			{
				_parent._action(value, _cancel);
				
				
				//todo schedule to main thread
				_observer.OnNotify(value);
				
			}, value);
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}


