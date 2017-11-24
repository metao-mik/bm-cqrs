using System;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public sealed class EventEnvelope
    {
        public EventEnvelope(Guid stream, Event @event, DateTime timestamp)
        {
            Stream = stream;
            Event = @event;
            Timestamp = timestamp;
        }

        public readonly Guid Stream;
        public readonly Event Event;
        public readonly DateTime Timestamp;
    }
}