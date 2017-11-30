using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Infrastruktur.Reactive;

namespace Billmorro.Infrastruktur.Implementierung
{
    public class InMemoryEventStore : EventStore
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<Eventset> _storage = new List<Eventset>();
        private readonly Func<DateTime> _clock;

        private int _letztes_eventset = 0;
        private readonly Subject<int> _eventsets = new Subject<int>();
        public IObservable<int> Eventsets => _eventsets;

        public int LastEventSetId => _letztes_eventset;

        public InMemoryEventStore(Func<DateTime> clock)
        {
            _clock = clock;
        }

        public IEnumerable<EventEnvelope> History(Guid stream)
        {
            _lock.EnterReadLock();
            try
            {
                return _storage.SelectMany(es=>es.Events.Where(_ => _.Stream == stream)).ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerable<Eventset> History()
        {
            _lock.EnterReadLock();
            try
            {
                return _storage.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Publish(List<UncommittedEvent> uncommittedEvents)
        {
            int eventsetid;
            _lock.EnterWriteLock();
            try
            {
                _letztes_eventset = _letztes_eventset + 1;
                eventsetid = _letztes_eventset;
                foreach (var @event in uncommittedEvents)
                {
                    Console.Out.WriteLine(@event.Event);
                }
                var eventset = new Eventset(eventsetid, new ReadOnlyCollection<EventEnvelope>(uncommittedEvents.Select(ue => new EventEnvelope(ue.Stream, ue.Event, _clock())).ToList()));
                _storage.Add(eventset);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            // Synchron: eigentlich neuer Thread aus Threadpool.
            _eventsets.Next(eventsetid);
        }
    }
}