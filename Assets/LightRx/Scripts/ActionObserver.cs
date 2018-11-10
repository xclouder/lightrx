using System;

public class ActionObserver<T> : IObserver<T>
{
	private readonly Action<T> _action;
	
	public ActionObserver(Action<T> act)
	{
		_action = act;
	}

	public void OnNotify(T value)
	{
		if (_action != null)
		{
			_action(value);
		}
	}

	public void OnError(Exception error)
	{
		
	}

}

public static class Observable_Action
{
	public static IDisposable Subscribe<T>(this IObservable<T> o, Action<T> action)
	{
		return o.Subscribe(new ActionObserver<T>(action));
	}
}
