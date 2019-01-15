using System;

namespace LightRx
{

    public class ActionObserver<T> : IObserver<T>
    {

        private Action<T> _onNextAction;
        private Action _onCompleteAction;
        private Action<Exception> _onErrAction;
	
        public ActionObserver(Action<T> onNext, Action onComplete = null, Action<Exception> onError = null)
        {
            _onNextAction = onNext;
            _onCompleteAction = onComplete;
            _onErrAction = onError;
        }
	
        public void OnNext(T value)
        {
            if (_onNextAction != null)
            {
                _onNextAction(value);    
            }
        }

        public void OnComplete()
        {
            if (_onCompleteAction != null)
            {
                _onCompleteAction();	
            }
		
        }

        public void OnError(Exception error)
        {
            if (_onErrAction != null)
            {
                _onErrAction(error);	
            }
		
        }
    }
}

