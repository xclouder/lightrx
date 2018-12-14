using System;
using System.Collections.Generic;

namespace LightRx
{
    public class CompositeDisposable : IDisposable
    {

        private readonly List<IDisposable> _disposables = new List<IDisposable>(10);

        public void Add(IDisposable d)
        {
            _disposables.Add(d);
        }
	
        public void Dispose()
        {
            foreach (var d in _disposables)
            {
                d.Dispose();
            }
        }
    }

}

