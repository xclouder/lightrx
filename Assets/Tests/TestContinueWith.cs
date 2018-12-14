using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightRx;
using LightRx.Unity;

public class TestContinueWith : MonoBehaviour
{

	private IDisposable cancel;
	// Use this for initialization
	void Start ()
	{
//		Observable.FromCoroutine<string>(DoAsync1)
//			.ContinueWith(v => Observable.FromCoroutine<string>(DoAsync2))
//			.Subscribe(val => Debug.Log(val));


		cancel = Observable.FromCoroutine(DoAsync3)
			.ContinueWith(_ => Observable.FromCoroutine(DoAsysnc4))
			.Subscribe(val =>
			{
				Debug.Log("subscribe onnext");
			}, () =>
			{
				Debug.Log("on completed");
			});

	}

	IEnumerator DoAsync1(IObserver<string> observer, CancellationToken cancellationToken)
	{
		int i = 0;
		while (i < 10)
		{
			i++;
			observer.OnNext(i.ToString());
			Debug.Log("async1 onnext");
			yield return new WaitForSeconds(1f);

		}
		
		observer.OnComplete();

	}
	
	IEnumerator DoAsync2(IObserver<string> observer, CancellationToken cancellationToken)
	{
		int i = 0;
		while (i < 10)
		{
			i++;
			observer.OnNext("2 >" + i.ToString());
			Debug.Log("async2 onnext");
			yield return new WaitForSeconds(1f);

		}
		
		observer.OnComplete();

	}

	IEnumerator DoAsync3(CancellationToken cancellationToken)
	{
		int i = 0;
		while (i < 3)
		{
			i++;
			Debug.Log("async1 onnext");
			yield return new WaitForSeconds(1f);

		}
		
	}

	IEnumerator DoAsysnc4(CancellationToken cancellationToken)
	{
		int i = 0;
		while (i < 3)
		{
			i++;
			Debug.Log("async2 onnext");
			yield return new WaitForSeconds(1f);

		}
	}

}
 