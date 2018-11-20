using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObserver<T> : IObserver<T>
{

	private static EmptyObserver<T> _ins;

	public static EmptyObserver<T> Instance
	{
		get { return _ins; }
	}
	
	public void OnNext(T value)
	{
		
	}

	public void OnComplete()
	{
		
	}

	public void OnError(Exception error)
	{
		
	}
	
}
