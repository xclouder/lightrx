using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

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
}
