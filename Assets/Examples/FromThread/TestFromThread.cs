using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using LightRx;
using LightRx.Unity;

public class TestFromThread : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Observable.FromCoroutine(Async1)
			.ContinueWith(v => Observable.FromSubThread(Async2, v))
			.Subscribe((v) => { Debug.Log("onNext"); },
				() => { Debug.Log("Completed");});
	}

	private IEnumerator Async1(CancellationToken cancellationToken)
	{
		Debug.Log("main thread:" + Thread.CurrentThread.ManagedThreadId);
		Debug.Log("coroutine wait 1 sec");
		yield return new WaitForSeconds(1);
		Debug.Log("coroutine wait 1 sec");
		yield return new WaitForSeconds(1);
		
	}

	private string Async2(Unit input, CancellationToken cancellationToken)
	{
		Debug.Log("sleep 1 sec, thread:" + Thread.CurrentThread.ManagedThreadId);
		Thread.Sleep(1000);
		
		return null;
	}
}


