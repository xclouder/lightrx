using System;

namespace LightRx
{

    public class SingleAssignmentDisposable : IDisposable
    {
        private IDisposable _disposable;

        public IDisposable Disposable
        {
            get { return _disposable; }
            set { _disposable = value; }
        }


        public SingleAssignmentDisposable()
        {
		
        }
		
        public SingleAssignmentDisposable(IDisposable disposable)
        {
            _disposable = disposable;
        }


        public bool IsDisposed
        {
            get { return _disposable == null; }
        }
	
	
        public void Dispose()
        {
            if (!IsDisposed)
            {
                _disposable.Dispose();	
            }
		
        }
    }

}
