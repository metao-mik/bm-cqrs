using System;
using System.Collections.Generic;
using Billmorro.Infrastruktur.Eventsourcing;

namespace Infrastruktur.Sqlite
{
    public class SqliteEventstore : EventStore
    {
        public IEnumerable<Eventset> History()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventEnvelope> History(Guid stream)
        {
            throw new NotImplementedException();
        }

        public void Publish(List<UncommittedEvent> uncommittedEvents)
        {
            throw new NotImplementedException();
        }

        public IObservable<int> Eventsets
        {
            get { throw new NotImplementedException(); }
        }

        public int LastEventSetId
        {
            get { throw new NotImplementedException(); }
        }
    }
}
