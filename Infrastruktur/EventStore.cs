using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur
{
    public interface EventStore
    {
        IEnumerable<EventEnvelope> History(Guid stream);
        void Publish(List<UncommittedEvent> uncommittedEvents);
    }
}