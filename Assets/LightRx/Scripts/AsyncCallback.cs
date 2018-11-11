using System;
using UnityEngine;

public class AsyncCallback<T, TR> : IObservable<TR>
{
	private readonly IObservable<T> _src;
	private readonly Action<T, Action<bool, TR, string>> _asyncCall;
	
	public AsyncCallback(IObservable<T> src, Action<T, Action<bool, TR, string>> asyncCall)
	{
		_src = src;
		_asyncCall = asyncCall;
	}
	
	public IDisposable Subscribe(IObserver<TR> observer)
	{
		var srcDis = _src.Subscribe(new InnerAsyncCallbackObserver<T,TR>(this, observer));
		var dispose = Disposable.CreateDefault(srcDis);
		
		return dispose;
	}

	private class InnerAsyncCallbackObserver<TInput, TReturn> : IObserver<TInput>
	{
		private readonly AsyncCallback<TInput, TReturn> _parent;
		private readonly IObserver<TReturn> _observer;
		
		public InnerAsyncCallbackObserver(AsyncCallback<TInput,TReturn> parent, IObserver<TReturn> observer)
		{
			_parent = parent;
			_observer = observer;
		}
		
		public void OnNotify(TInput value)
		{
			_parent._asyncCall(value, (succ, result, errmsg) =>
			{
				if (succ)
				{
					_observer.OnNotify(result);
				}
				else
				{
					//fail
					Debug.LogError("do async call returns error:" + errmsg);
				}
			});

		}

		public void OnError(Exception error)
		{
			//throw new NotImplementedException();
		}
	}
	
}
