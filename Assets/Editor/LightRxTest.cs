using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using LightRx;

public class LightRxTest {

	[Test]
	public void LightRxTestSimplePasses()
	{
		AsyncTask.Create<string, Object>("test", LoadResAsync)
			.AsyncTask<Object, int>(CreateObject);
		//.AsyncTask(int, string)
	}

	IObservable<int> CreateObservable(List<int> list)
	{
		return null;
	}

	IObserver<int> CreateObserver()
	{
		return null;
	}


	private void LoadResAsync(string path, System.Action<UnityEngine.Object> cb)
	{
		Debug.Log("loadRes:" + path);

		Resources.LoadAsync<Object>(path);

		cb(null);
	}

	private void CreateObject(Object o, System.Action<int> cb)
	{
		//TODO
	}

}
