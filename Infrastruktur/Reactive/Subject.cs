using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur.Reactive
{
    public class Subject<T> : IObservable<T>
    {

        public Subject(bool memorizeLast=false){
            _memorize_last = memorizeLast;
        }
        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();
        private bool _memorize_last;
        private bool _has_last = false;
        private T _last = default(T);

        public void Next(T t)
        {
            if (_memorize_last){
                _last = t;
                _has_last = true;
            }
            foreach (var observer in _observers)
            {
                observer.OnNext(t);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            if (_has_last) observer.OnNext(_last);
            return new Unsubscriber(() =>
            {
                if (_observers.Contains(observer)) _observers.Remove(observer);
            });
        }

        private class Unsubscriber : IDisposable
        {
            private Action _action;

            public Unsubscriber(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                var action = _action;
                _action = null;
                action?.Invoke();
            }
        }
    }
}