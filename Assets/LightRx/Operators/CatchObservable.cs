using System;
using LightRx;

namespace LightRx
{
    
    public class CatchObservable<T> : IObservable<T> {
	
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return null;
        }
	
    }

}

