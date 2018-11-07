

using System;

namespace LightRx
{
	
    public interface IObserver<T>
    {

	    void OnCompleted();
	    void OnError(Exception e);
	    void OnNext(T value);

    }

}

