using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Test {

	[Test]
	public void TestSimplePasses()
	{
		
		var intValOb = new TestIntObservable(0);
		
		var disposable = intValOb.Where(val => val > 1)
			.Subscribe(msg => Debug.Log("msg:" + msg));
		
		disposable.Dispose();

	}
	
}

class TestIntObservable : IObservable<int>
{
	private int _val;
	
	public TestIntObservable(int initVal)
	{
		_val = initVal;
	}
	
	public IDisposable Subscribe(IObserver<int> observer)
	{
		observer.OnNotify(_val);

		return Disposable.CreateDefault(null);
	}
}