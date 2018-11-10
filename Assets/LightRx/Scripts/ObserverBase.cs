using System;

public class ObserverBase<T> : IObserver<T> {
	
	public void OnNotify(T value)
	{
		
	}

	public void OnError(Exception error)
	{
		throw new NotImplementedException();
	}
	
}
