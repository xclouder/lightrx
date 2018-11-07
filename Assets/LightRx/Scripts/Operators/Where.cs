using System;
using LightRx;

public class Where<T> : OperatorObserverableBase<T>
{

	private readonly IObservable<T> source;
	private readonly Func<T, bool> predicate;

	public Where(IObservable<T> source, Func<T, bool> predicate)
	{
		this.source = source;
		this.predicate = predicate;
	}
	
	protected override ICancelable SubscribeCore(IObserver<T> observer, ICancelable cancelable)
	{
		return source.Subscribe(new InnerWhereObserver(observer, cancelable, predicate));
	}

	private class InnerWhereObserver : OperatorObserverBase<T, T>
	{
		private readonly Func<T, bool> predicator;
		public InnerWhereObserver(IObserver<T> observer, ICancelable cancel, Func<T, bool> predicator) : base(observer, cancel)
		{
			this.predicator = predicator;
		}

		public override void OnCompleted()
		{
			try { observer.OnCompleted(); } finally { /*Dispose();*/ }
		}

		public override void OnError(Exception e)
		{
			try { observer.OnError(e); } finally { /*Dispose();*/ }
		}

		public override void OnNext(T value)
		{
			var isPassed = false;
			try
			{
				isPassed = predicator(value);
			}
			catch (Exception ex)
			{
				try { observer.OnError(ex); } finally { /*Dispose();*/ }
				return;
			}

			if (isPassed)
			{
				observer.OnNext(value);
			}
		}
	}
}
