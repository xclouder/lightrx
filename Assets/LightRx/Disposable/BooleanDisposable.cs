using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanDisposable : IDisposable, ICancelable {
	
	public bool IsCanceled { get; private set; }

	public BooleanDisposable()
	{

	}

	internal BooleanDisposable(bool isDisposed)
	{
		IsCanceled = isDisposed;
	}

	public void Dispose()
	{
		if (!IsCanceled) IsCanceled = true;
	}
	
}
