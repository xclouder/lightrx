using System;

public class ObservableBase<TSource, TResult> : IObservable<TResult>
{
    private IObservable<TSource> _src;
    
    public ObservableBase(IObservable<TSource> src)
    {
        _src = src;
    }

    public IDisposable Subscribe(IObserver<TResult> observer)
    {
        var srcDisposable = _src.Subscribe(new InnerObserver<TSource, TResult>(observer));
        var disposable = Disposable.CreateDefault(srcDisposable);

        return disposable;
    }

}

internal class InnerObserver<TSource, TResult> : IObserver<TSource>
{
    private IObserver<TResult> _target;
    public InnerObserver(IObserver<TResult> target)
    {
        _target = target;
    }
    
    public void OnNotify(TSource value)
    {
        var result = default(TResult);
        _target.OnNotify(result);
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
}

