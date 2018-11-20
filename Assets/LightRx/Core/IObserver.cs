using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver<T>
{

	void OnNext(T value);
	void OnComplete();
	void OnError(Exception error);

}
