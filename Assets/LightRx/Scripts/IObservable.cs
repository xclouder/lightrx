using System;

namespace LightRx
{
    public interface IObservable<T> {

	    ICancelable Subscribe(IObserver<T> observer);
	
    }

}


