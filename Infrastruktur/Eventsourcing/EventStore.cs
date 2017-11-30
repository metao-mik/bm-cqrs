using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public interface EventStore
    {
        IEnumerable<Eventset> History();
        IEnumerable<EventEnvelope> History(Guid stream);
        void Publish(List<UncommittedEvent> uncommittedEvents);
        IObservable<int> Eventsets { get; }
        int LastEventSetId { get; }
    }
}