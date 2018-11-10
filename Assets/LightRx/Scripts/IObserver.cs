using System;

public interface IObserver<T>
{

	void OnNotify(T value);

	void OnError(Exception error);
	
//	void Complete();

}