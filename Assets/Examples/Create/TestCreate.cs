using UnityEngine;
using LightRx;

public class TestCreate : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Observable.Create<string>((observer) =>
		{
			observer.OnNext("abc");
			observer.OnNext("def");
			observer.OnComplete();
			return Disposable.Empty;
		}).Subscribe(val =>
		{
			Debug.Log("onNext:" + val);
		}, () =>
		{
			Debug.Log("onComplete");
		});
	}

}
