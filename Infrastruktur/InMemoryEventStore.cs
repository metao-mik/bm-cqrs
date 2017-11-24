using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Billmorro.Infrastruktur
{
    public class InMemoryEventStore : EventStore
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<EventEnvelope> _storage = new List<EventEnvelope>();
        private readonly Func<DateTime> _clock;

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
            _lock.EnterWriteLock();
            try
            {
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
        }
    }
}