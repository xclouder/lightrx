using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disposable {

	public static readonly IDisposable Empty = EmptyDisposable.Singleton;
	
	class EmptyDisposable : IDisposable
	{
		public static readonly EmptyDisposable Singleton = new EmptyDisposable();

		private EmptyDisposable()
		{

		}

		public void Dispose()
		{
		}
	}
	
}
