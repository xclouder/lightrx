using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FromSubThread<T, TR> : IObservable<TR>
{
	private IObservable<T> _source;
	private Func<T, CancellationToken, TR> _execFunc;
	
	public FromSubThread(IObservable<T> source, Func<T, CancellationToken, TR> execFunc)
	{
		_source = source;
		_execFunc = execFunc;
	}
	
	public IDisposable Subscribe(IObserver<TR> observer)
	{
		
		
		return null;
	}

	private class FromSubThreadInternal : OperatorObserverBase<T, TR>
	{
		public FromSubThreadInternal(IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
		{
			
		}

		public IDisposable Run(Func<T, CancellationToken, TR> execFunc)
		{
			MainThreadDispatcher.Instance.StartCoroutine(ExecFuncCo(execFunc));
			return null;
		}

		private IEnumerator ExecFuncCo(Func<T, CancellationToken, TR> execFunc)
		{
			Exception throwedException = null;
			bool finished = false;
			TR result = default(TR);
			
			//TODO: 这里T怎么传进来？
			ThreadPool.QueueUserWorkItem(o =>
			{
				try
				{
					//TODO cancel
					result = execFunc(default(T), new CancellationToken());
				}
				catch (Exception e)
				{
					Debug.LogException(e);
					throwedException = e;
				}

				finished = true;
			});

			while (!finished)
			{
				yield return null;
			}

			if (throwedException == null)
			{
				Observer.OnNext(result);
				Observer.OnError(throwedException);
			}
			else
			{
				Observer.OnComplete();
			}

		}

		public override void OnNext(T value)
		{
			//do nothing
		}

		public override void OnComplete()
		{
			
		}

		public override void OnError(Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
