using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightRx
{
    public interface ISubject<TSource, TResult>
    {
        
    }
    
    public interface ISubject<T> : ISubject<T, T> 
    {

    }
}

