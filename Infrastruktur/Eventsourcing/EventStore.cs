using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public interface EventStore
    {
        IEnumerable<EventEnvelope> History(Guid stream);
        void Publish(List<UncommittedEvent> uncommittedEvents);
        IObservable<int> Eventsets { get; }
    }
}