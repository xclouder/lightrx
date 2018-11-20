using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAssignmentDisposable : IDisposable
{
	private IDisposable _disposable;
	
	public SingleAssignmentDisposable(IDisposable disposable)
	{
		_disposable = disposable;
	}


	public bool IsDisposed
	{
		get { return _disposable == null; }
	}
	
	
	public void Dispose()
	{
		if (!IsDisposed)
		{
			_disposable.Dispose();	
		}
		
	}
}
