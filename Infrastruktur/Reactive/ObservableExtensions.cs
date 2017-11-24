using System;

namespace Billmorro.Infrastruktur.Reactive
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext) =>
            observable.Subscribe(new Observer<T>(onNext, ex => { throw ex; }));
    }
}