using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using LightRx;
using LightRx.Unity;

public class TestFromCoroutine {


	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator TestNormalUseage() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		bool finish = false;

		var cancel = Observable.FromCoroutine<int>(TestCoroutine)
			.Subscribe(val =>
			{
				Debug.Log("got val:" + val);
			},
			() =>
			{
				Debug.Log("completed");
				finish = true;
			}
			);

		int i = 0;
		while (!finish && i < 3000)
		{
			i++;

			if (i > 500)
			{
				Debug.Log("i > 500, cancel it");
				cancel.Dispose();
			}
			
			yield return null;	
		}
		
	}

	private IEnumerator TestCoroutine(IObserver<int> observer, CancellationToken cancellationToken)
	{
		int i = 0;
		while (!cancellationToken.IsCancellationRequested)
		{
			observer.OnNext(i++);
			yield return new WaitForSeconds(1f);
		}
		
		observer.OnComplete();
	}


	[UnityTest]
	public IEnumerator TestWhenAll()
	{
		bool succ = false;
		
		var o1 = Observable.FromCoroutine<string>(TestO1);
		var o2 = Observable.FromCoroutine<string>(TestO2);

		var whenAll = Observable.WhenAll(o1, o2);
		var cancel = whenAll.Subscribe(values =>
		{
			foreach (var v in values)
			{
				Debug.Log("val:" + v);
			}
		}, () =>
		{
			succ = true;
			Debug.Log("on complete");
		}, (err) =>
		{
			Debug.LogException(err);
		});

		int k = 0;
		while (!succ && k < 200)
		{
			k++;
			Debug.Log("k:" + k);
			yield return null;
		}

//		cancel.Dispose();
		Debug.Log("finish");
	}

	IEnumerator TestO1(IObserver<string> ob, CancellationToken cancellationToken)
	{
		int i = 0;
		while (!cancellationToken.IsCancellationRequested && i < 100)
		{
			i++;
			Debug.Log("onnext:A, i:" + i);
			ob.OnNext("A");

			if (i == 50)
			{
				ob.OnError(new Exception("A throw exception"));
				yield break;
			}

			yield return null;

		}

//		if (cancellationToken.IsCancellationRequested)
//		{
//			ob.OnError(new Exception("A cancelled"));
//			yield break;
//		}
		
		ob.OnComplete();
	}
	
	IEnumerator TestO2(IObserver<string> ob, CancellationToken cancellationToken)
	{
		int i = 0;

		while (!cancellationToken.IsCancellationRequested && i < 100)
		{
			i++;
			Debug.Log("onnext:B, i:" + i);
			ob.OnNext("B");
			yield return null;
		}
		
		ob.OnComplete();
	}
}
