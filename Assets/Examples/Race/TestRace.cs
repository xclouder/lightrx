using System.Collections;
using LightRx;
using UnityEngine;

public class TestRace : MonoBehaviour {

	void Start()
	{
		Observable.Race(
				ObservableWWW.Get("http://www.zhihu.com"),
				ObservableWWW.Get("http://www.qq.com"),
				Observable.FromCoroutine<string>(GetStrCo)
			)
			.Subscribe(v =>
			{
				Debug.Log("get content:" + v);
			}, () => { Debug.Log("completed");}, e => Debug.LogException(e));
	}

	IEnumerator GetStrCo(IObserver<string> observer, CancellationToken cancellationToken)
	{
		
		yield return new WaitForSeconds(1f);
        		
		if (cancellationToken.IsCancellationRequested)
		{
			Debug.LogError("coroutine canceled");
			yield break;
		}

		Debug.Log("coroutine finish");

		observer.OnNext("finished co");
		observer.OnComplete();
	}
	
}
