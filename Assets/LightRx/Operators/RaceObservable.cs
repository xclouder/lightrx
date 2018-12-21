using System;
using System.Collections;
using System.Collections.Generic;
using LightRx;
using UnityEngine;

namespace LightRx
{
	public enum RaceMode
	{
		OnNext,		//TODO 此模式暂时不实现
		CompleteOrError,
	}
	
	public class RaceObservable<T> : IObservable<T> {

		//private RaceMode _raceMode = RaceMode.CompleteOrError;
		private readonly IObservable<T>[] _observables;
		
	    public RaceObservable(RaceMode raceMode, params IObservable<T>[] observables)
	    {
		    //_raceMode = raceMode;
		    _observables = observables;
	    }
	    
	    public IDisposable Subscribe(IObserver<T> observer)
	    {
		    var cancel = new SingleAssignmentDisposable();
		    var innerObserver = new InnerRaceObserver(_observables, observer, cancel);
	
		    cancel.Disposable = innerObserver.Run();
	
		    return cancel;
	    }

	    private class InnerRaceObserver : OperatorObserverBase<T, T>
	    {
		    private readonly IObservable<T>[] _sources;
		    private T _lastSeenVal;
		    private bool _seenValue = false;
		    private bool _hasOneFinished = false;
		    
		    public InnerRaceObserver(IObservable<T>[] sources, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
		    {
			    _sources = sources;
		    }

		    public IDisposable Run()
		    {
			    if (_sources.Length == 0)
			    {
				    OnNext(default(T));
				    try { Observer.OnComplete(); } finally { Dispose(); }
					
				    return Disposable.Empty;
			    }
			    
			    var disposable = new CompositeDisposable();
			    for (int index = 0; index < _sources.Length; index++)
			    {
				    var source = _sources[index];
				    var observer = new RaceCollectionObserver(this, index);
				    var d = source.Subscribe(observer);
					
				    disposable.Add(d);
			    }

			    _cancel = disposable;
				
			    return disposable;
		    }
		    
		    public override void OnNext(T value)
		    {
			    if (!_hasOneFinished)
			    {
				    _lastSeenVal = value;
				    _seenValue = true;
				    
				    //if we need OnNext, determine by mode
				    //Observer.OnNext(value);
			    }
		    }

		    public override void OnComplete()
		    {
			    if (!_hasOneFinished)
			    {
				    _hasOneFinished = true;
				    try
				    {
					    if (_seenValue)
					    {
						    Observer.OnNext(_lastSeenVal);    
					    }
				    
					    Observer.OnComplete();
				    }
				    finally 
				    {
					    Dispose();
				    }
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
		    
		    
		    
	    }

		private class RaceCollectionObserver : IObserver<T>
		{

			private readonly InnerRaceObserver _parent;
			//private readonly int _index;
			private bool _isCompleted = false;
			private T _lastSeenVal;
			
			public RaceCollectionObserver(InnerRaceObserver parent, int index)
			{
				_parent = parent;
				//_index = index;
			}
			
			public void OnNext(T value)
			{
				if (!_isCompleted)
				{
					_lastSeenVal = value;
				}
			}

			public void OnComplete()
			{
				if (!_isCompleted)
				{
					_isCompleted = true;
					
					_parent.OnNext(_lastSeenVal);
					_parent.OnComplete();
					    
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

