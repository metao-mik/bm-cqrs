using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Infrastruktur.Reactive;

namespace Billmorro.Infrastruktur.Implementierung
{
    public class InMemoryEventStore : EventStore
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<EventEnvelope> _storage = new List<EventEnvelope>();
        private readonly Func<DateTime> _clock;

        private int _letztes_eventset = 0;
        private readonly Subject<int> _eventsets = new Subject<int>();
        public IObservable<int> Eventsets => _eventsets;

        public InMemoryEventStore(Func<DateTime> clock)
        {
            _clock = clock;
        }

        public IEnumerable<EventEnvelope> History(Guid stream)
        {
            _lock.EnterReadLock();
            try
            {
                return _storage.Where(_ => _.Stream == stream).ToList();
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
                _storage.AddRange(uncommittedEvents.Select(ue=>new EventEnvelope(ue.Stream,ue.Event,_clock())));
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