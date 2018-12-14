using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace LightRx.Unity
{

	public class FromSubThreadObservable<T, TR> : IObservable<TR>
	{
		private IObservable<T> _source;
		private readonly Func<T, CancellationToken, TR> _execFunc;
		private readonly T _input;
		
		public FromSubThreadObservable(Func<T, CancellationToken, TR> execFunc, T inputVal = default(T))
		{
			_execFunc = execFunc;
			_input = inputVal;
		}
		
		public IDisposable Subscribe(IObserver<TR> observer)
		{
			return new FromSubThreadInternal(observer, null).Run(_input, _execFunc);
		}
	
		private class FromSubThreadInternal : OperatorObserverBase<T, TR>
		{
			public FromSubThreadInternal(IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
			{
			}
	
			public IDisposable Run(T inputVal, Func<T, CancellationToken, TR> execFunc)
			{
				var disposable = new BooleanDisposable();
				var cancellationToken = new CancellationToken(disposable);
				_cancel = disposable;
				
				MainThreadDispatcher.Instance.StartCoroutine(ExecFuncCo(inputVal, cancellationToken, execFunc));
				return disposable;
			}
	
			private IEnumerator ExecFuncCo(T inputVal, CancellationToken cancellationToken, Func<T, CancellationToken, TR> execFunc)
			{
				Exception throwedException = null;
				bool finished = false;
				TR result = default(TR);
				var input = inputVal;
				
				ThreadPool.QueueUserWorkItem(o =>
				{
					try
					{
						result = execFunc(input, cancellationToken);
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
					Observer.OnComplete();
				}
				else
				{
					Observer.OnError(throwedException);
				}
	
			}
	
			public override void OnNext(T value)
			{
				//do nothing
			}
	
			public override void OnComplete()
			{
				try
				{
					Observer.OnComplete();
				}
				catch
				{
					Dispose();
					
					throw;
				}
			}
	
			public override void OnError(Exception error)
			{
				try
				{
					Observer.OnError(error);
				}
				catch
				{
					Dispose();
					throw;
				}
			}
		}
	}

	
}
