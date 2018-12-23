using LightRx;
using System;

public class TaskExecutor<T> where T : class
{

	private IObservable<T> _lastObservable;
	private PublishSubject<T> _head;
	
	public void AddTask(IObservable<T> observable)
	{
		if (_lastObservable == null)
		{
			_head = new PublishSubject<T>();
			_lastObservable = _head;
		}
		else
		{
			var last = _lastObservable.ContinueWith(v => observable);
			_lastObservable = last;
		}
	}

	private T _lastVal;
	private Action<T> _onCompleted;
	public void Start(T input, Action<T> onComplete)
	{
		_onCompleted = onComplete;
		_lastObservable.Subscribe(UpdateVal, OnComplete);
		
		_head.OnNext(input);
		_head.OnComplete();
	}

	private void UpdateVal(T val)
	{
		_lastVal = val;
	}

	private void OnComplete()
	{
		if (_onCompleted != null)
		{
			_onCompleted(_lastVal);
		}
	}
	
}
