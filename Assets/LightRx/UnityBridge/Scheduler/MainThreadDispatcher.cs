using System;
using System.Collections;
using UnityEngine;

public class MainThreadDispatcher
{
	private static MainThreadDispatcher _ins;
	public static MainThreadDispatcher Instance
	{
		get
		{
			Initialize();
			return _ins;
		}
	}

	private static void Initialize()
	{
		if (_ins == null)
		{
			_ins = new MainThreadDispatcher();
		}
	}

	private readonly InnerHelpMonoBehaviour _monoBehaviour;
	private MainThreadDispatcher()
	{
		var obj = new GameObject("[LightRx-MainThreadDispatcher]");
		obj.hideFlags = HideFlags.HideInHierarchy;
		GameObject.DontDestroyOnLoad(obj);
		
		_monoBehaviour = obj.AddComponent<InnerHelpMonoBehaviour>();
	}

	public void StartCoroutine(IEnumerator coroutine)
	{
		if (InnerHelpMonoBehaviour.mainThreadToken == null)
		{
			throw new InvalidOperationException("can only call this method in main thread");
		}

		_monoBehaviour.StartCoroutine(coroutine);
	}
	
	private class InnerHelpMonoBehaviour : MonoBehaviour
	{
		[ThreadStatic]
		public static object mainThreadToken;

		private void Awake()
		{
			mainThreadToken = new object();
		}
	}
	
}
