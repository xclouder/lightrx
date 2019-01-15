using System.Collections;
using System.Collections.Generic;
using LightRx;
using UnityEngine;

public class TestFromAction : MonoBehaviour {

	void Start()
	{
		Observable.FromAtion((onComplete) =>
		{
			var o = GameObject.Instantiate(Resources.Load("Test"));
			onComplete();
		})
		.ContinueWithAction((onComplete) =>
			{
				var o = GameObject.Instantiate(Resources.Load("Test2"));
				onComplete();
			})
			.Subscribe(null, () =>
			{
				Debug.LogWarning("Finish");
			});
		
		
	}

	
}
