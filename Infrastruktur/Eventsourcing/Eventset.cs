using System.Collections.Generic;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public struct Eventset
    {
        public Eventset(int id, IReadOnlyCollection<EventEnvelope> events)
        {
            Events = events;
            Id = id;
        }
        public readonly int Id;
        public readonly IReadOnlyCollection<EventEnvelope> Events;
    }
}