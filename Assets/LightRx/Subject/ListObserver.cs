using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListObserver<T> : IObserver<T>
{

	private Dictionary<IObserver<T>, int> _dic;
	private List<IObserver<T>> _list;

	public bool HasObservers
	{
		get
		{
			return (_dic != null && _dic.Count > 0);
		}
		
	}
	
	private void InitIfNeeds()
	{
		if (_dic == null)
		{
			_dic = new Dictionary<IObserver<T>, int>(8);
			_list = new List<IObserver<T>>(5);
		}
	}
	
	public void AddObserver(IObserver<T> observer)
	{
		InitIfNeeds();

		var index = _list.Count;
		_list.Add(observer);
		_dic.Add(observer, index);
	}

	public void RemoveObserver(IObserver<T> observer)
	{
		InitIfNeeds();

		var index = 0;
		if (_dic.TryGetValue(observer, out index))
		{
			_list.RemoveAt(index);
			_dic.Remove(observer);
		}
			
	}
	

	public void OnNext(T value)
	{
		if (_list == null || _list.Count == 0)
		{
			return;
		}

		foreach (var o in _list)
		{
			o.OnNext(value);
		}
	}

	public void OnComplete()
	{
		if (_list == null || _list.Count == 0)
		{
			return;
		}

		foreach (var o in _list)
		{
			o.OnComplete();
		}
	}

	public void OnError(Exception error)
	{
		if (_list == null || _list.Count == 0)
		{
			return;
		}

		foreach (var o in _list)
		{
			o.OnError(error);
		}
	}
	
}
