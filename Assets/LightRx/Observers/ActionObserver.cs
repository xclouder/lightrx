using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObserver<T> : IObserver<T>
{

	private Action<T> _onNextAction;
	private Action _onCompleteAction;
	private Action<Exception> _onErrAction;
	
	public ActionObserver(Action<T> onNext, Action onComplete = null, Action<Exception> onError = null)
	{
		_onNextAction = onNext;
		_onCompleteAction = onComplete;
		_onErrAction = onError;
	}
	
	public void OnNext(T value)
	{
		_onNextAction(value);
	}

	public void OnComplete()
	{
		_onCompleteAction();
	}

	public void OnError(Exception error)
	{
		_onErrAction(error);
	}
}
