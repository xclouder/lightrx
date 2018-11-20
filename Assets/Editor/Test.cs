using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Threading;

public class Test {

	[Test]
	public void TestSimplePasses()
	{
		
		var intValOb = new TestIntObservable(6);

		var o1 = intValOb.Where(val => val > 1)
			.Select(val => val + ".tostring");
//			.ScheduleToThread((val, cancel) =>
//			{
//
//				while (true)
//				{
//					Debug.Log("sleep 1000ms");
//					Thread.Sleep(1000);
//					
////					if (cancel.State == CancelState.Cancelling)
////					{
////						Debug.Log("cancelled");
////						return;
////					}
//				}
//				
//			});
		var d1 = o1.Subscribe(msg => Debug.Log("msg:" + msg));
		var d2 = o1.Subscribe(msg => Debug.Log("msg2:" + msg));

		d1.Dispose();
		d2.Dispose();
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
		observer.OnNext(_val);

		//return Disposable.CreateDefault(null);
		//TODO
		return null;
	}
}