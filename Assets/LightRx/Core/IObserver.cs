using System;

namespace LightRx
{

	public interface IObserver<T>
	{

		void OnNext(T value);
		void OnComplete();
		void OnError(Exception error);

	}

}
