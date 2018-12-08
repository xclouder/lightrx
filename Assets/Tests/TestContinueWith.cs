using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContinueWith : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Observable.FromCoroutine<string>(DoAsync1)
			.ContinueWith(v => Observable.FromCoroutine<string>(DoAsync2))
			.Subscribe(val => Debug.Log(val));
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

}
 