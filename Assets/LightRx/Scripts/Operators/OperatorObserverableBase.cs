using LightRx;

public abstract class OperatorObserverableBase<T> : IObservable<T>
{
	public ICancelable Subscribe(IObserver<T> observer)
	{
		return SubscribeCore(observer, null);
	}

	protected abstract ICancelable SubscribeCore(IObserver<T> observer, ICancelable cancelable);

}
