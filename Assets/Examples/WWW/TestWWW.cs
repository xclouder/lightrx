using System.Collections;
using System.Collections.Generic;
using LightRx;
using UnityEngine;

public class TestWWW : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		ObservableWWW.Get("https://www.zhihu.com/").Subscribe(c =>
		{
			Debug.Log("get content:" + c);
		}, () =>
		{
			Debug.Log("completed");
		}, e =>
		{
			Debug.LogException(e);
		});
	}
	
}
