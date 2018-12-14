using LightRx;
using NUnit.Framework;
using UnityEngine;

public class Test {

	[Test]
	public void TestSimplePasses()
	{
		
		var sub = new PublishSubject<int>();

		sub.Where(v => v > 3)
			.Select(v => v.ToString())
			.Subscribe(v => Debug.Log("result:" + v));
		
		sub.OnNext(1);
		sub.OnNext(3);
		sub.OnNext(9);

	}
	
}
