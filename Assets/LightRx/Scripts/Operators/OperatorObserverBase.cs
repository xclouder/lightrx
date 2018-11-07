using System;
using LightRx;

public abstract class OperatorObserverBase<TSource, TResult> : IObserver<TSource>
{
	ICancelable cancel;
	protected IObserver<TResult> observer;

	public OperatorObserverBase(IObserver<TResult> observer, ICancelable cancel)
	{
		this.observer = observer;
		this.cancel = cancel;
	}

	public abstract void OnCompleted();

	public abstract void OnError(Exception e);

	public abstract void OnNext(TSource value);

}
