using System.Collections;
using System.Collections.Generic;
using LightRx;
using UnityEngine;

public class TestRace : MonoBehaviour {

	void Start()
	{
		Observable.Race(
				ObservableWWW.Get("http://www.zhihu.com"),
				ObservableWWW.Get("http://www.qq.com")
			)
			.Subscribe(v =>
			{
				Debug.Log("get content:" + v);
			});
	}
	
}
