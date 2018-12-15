using System;

namespace LightRx
{

	public class WhenAllObservable<T> : IObservable<T[]>
	{
	
		private readonly IObservable<T>[] _observables;
	
		public WhenAllObservable(params IObservable<T>[] observables)
		{
			_observables = observables;
		}
		
		public IDisposable Subscribe(IObserver<T[]> observer)
		{
			var cancel = new SingleAssignmentDisposable();
			
			var innerObserver = new InnerWhenAllObserver(_observables, observer, cancel);
	
			cancel.Disposable = innerObserver.Run();
	
			return cancel;
		}
	
		private class InnerWhenAllObserver : OperatorObserverBase<T[], T[]>
		{
			private readonly IObservable<T>[] _sources;
	
			private T[] _values;
			private int _length;
			private int _completedCount;
			
			public InnerWhenAllObserver(IObservable<T>[] sources, IObserver<T[]> observer, IDisposable cancel) : base(observer, cancel)
			{
				_sources = sources;
			}
	
			public override void OnNext(T[] value)
			{
				Observer.OnNext(value);
			}
	
			public override void OnComplete()
			{
				try
				{
					Observer.OnComplete();
				}
				finally 
				{
					Dispose();
				}
			}
	
			public override void OnError(Exception error)
			{
				try
				{
					Observer.OnError(error);
				}
				finally 
				{
					Dispose();
				}
			}
	
			public IDisposable Run()
			{
				_length = _sources.Length;
	
				if (_length == 0)
				{
					OnNext(new T[0]);
					try { Observer.OnComplete(); } finally { Dispose(); }
					
					return Disposable.Empty;
				}
				
				_completedCount = 0;
				_values = new T[_length];
	
				var disposable = new CompositeDisposable();
				for (int index = 0; index < _length; index++)
				{
					var source = _sources[index];
					var observer = new WhenAllCollectionObserver(this, index);
					var d = source.Subscribe(observer);
					
					disposable.Add(d);
				}

				_cancel = disposable;
				return disposable;
			}
			
			private class WhenAllCollectionObserver : IObserver<T>
			{
				private readonly InnerWhenAllObserver _parent;
				private readonly int _index;
				private bool _isCompleted = false;
				
				public WhenAllCollectionObserver(InnerWhenAllObserver parent, int index)
				{
					_parent = parent;
					_index = index;
				}
				
				public void OnNext(T value)
				{
					if (!_isCompleted)
					{
						_parent._values[_index] = value;
					}
				}
	
				public void OnComplete()
				{
					if (!_isCompleted)
					{
						_isCompleted = true;
						_parent._completedCount++;
						
						if (_parent._completedCount == _parent._length)
						{
							_parent.OnNext(_parent._values);
							_parent.OnComplete();
						}
					}
				}
	
				public void OnError(Exception error)
				{
					if (!_isCompleted)
					{
						_parent.OnError(error);
					}
				}
			}
		}
		
	}

}
