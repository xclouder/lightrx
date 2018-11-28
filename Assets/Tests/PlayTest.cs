using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(TestWhenAll());
	}
	
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
		while (!succ && k < 30)
		{
			k++;
			Debug.Log("k:" + k);
			yield return null;
		}

		cancel.Dispose();
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
				ob.OnError(new System.Exception("A throw exception"));
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
