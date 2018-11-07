using System;
using LightRx;

public class AsyncTask<TSource, TResult> : OperatorObserverableBase<TResult>
{
	private readonly TSource inputVal;
	private readonly Action<TSource, Action<TResult>> taskExecutor;
	private readonly IObservable<TSource> source;
	
	public AsyncTask(IObservable<TSource> source, Action<TSource, Action<TResult>> taskExecutor)
	{
		this.source = source;
		this.taskExecutor = taskExecutor;
	}

	public AsyncTask(TSource inputVal, Action<TSource, Action<TResult>> taskExecutor)
	{
		this.inputVal = inputVal;
		this.taskExecutor = taskExecutor;
	}
	
	protected override ICancelable SubscribeCore(IObserver<TResult> observer, ICancelable cancelable)
	{
		if (source == null)
		{
			var ob = new InnerAsyncTaskObserverWithoutSource(this, observer, cancelable);
			
			ob.OnNext(inputVal);
			
			return null;
		}
		else
		{
			return source.Subscribe(new InnerAsyncTaskObserver(this, observer, cancelable));	
		}
		
	}

	private class InnerAsyncTaskObserver : OperatorObserverBase<TSource, TResult>
	{
		private readonly AsyncTask<TSource, TResult> parent;
		public InnerAsyncTaskObserver(AsyncTask<TSource, TResult> parent, IObserver<TResult> observer, ICancelable cancel) : base(observer, cancel)
		{
			this.parent = parent;
		}

		public override void OnCompleted()
		{
			observer.OnCompleted();
		}

		public override void OnError(Exception e)
		{
			observer.OnError(e);
		}

		public override void OnNext(TSource value)
		{
			parent.taskExecutor(value, result =>
			{
				observer.OnNext(result);
			});
		}
	}
	
	
	private class InnerAsyncTaskObserverWithoutSource : OperatorObserverBase<TSource, TResult>
	{
		private readonly AsyncTask<TSource, TResult> parent;
		public InnerAsyncTaskObserverWithoutSource(AsyncTask<TSource, TResult> parent, IObserver<TResult> observer, ICancelable cancel) : base(observer, cancel)
		{
			this.parent = parent;
		}

		public override void OnCompleted()
		{
			observer.OnCompleted();
		}

		public override void OnError(Exception e)
		{
			observer.OnError(e);
		}

		public override void OnNext(TSource value)
		{
			parent.taskExecutor(value, result =>
			{ 
				observer.OnNext(result);
			});
		}
	}

}

public class AsyncTask
{
	public static AsyncTask<T, TR> Create<T, TR>(T taskInput, Action<T, Action<TR>> executor)
	{
		var o = new AsyncTask<T, TR>(taskInput, executor);
		return o;
	}
}
