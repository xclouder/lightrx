using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LightRx;
using UnityEngine;

public class TestWhenAll : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Observable.WhenAll(
			Observable.FromCoroutine<string>(Async1),
			Observable.FromSubThread<string, string>(Async2),
			ObservableWWW.Get("http://www.zhihu.com")
			)
			.Subscribe((list) =>
			{
				foreach (var c in list)
				{
					Debug.Log("data:" + c);
				}
			});
	}

	IEnumerator Async1(IObserver<string> observer, CancellationToken cancellationToken)
	{
		int sec = 0;
		while (sec < 3 && !cancellationToken.IsCancellationRequested)
		{
			yield return new WaitForSeconds(1f);
			sec++;
			
			observer.OnNext(sec.ToString());
		}

		if (!cancellationToken.IsCancellationRequested)
		{
			observer.OnComplete();
		}
	}

	string Async2(string input, CancellationToken cancellationToken)
	{
		int sleepedTime = 0;
		while (!cancellationToken.IsCancellationRequested && sleepedTime < 1000)
		{
			Thread.Sleep(100);
			sleepedTime += 100;
		}
		
		return "async2 complete";
	}
	
}
