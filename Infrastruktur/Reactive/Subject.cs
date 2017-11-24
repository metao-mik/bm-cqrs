﻿using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur.Reactive
{
    public class Subject<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();

        public void Next(T t)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(t);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
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