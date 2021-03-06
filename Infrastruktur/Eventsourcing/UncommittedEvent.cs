using System;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public struct UncommittedEvent
    {
        public UncommittedEvent(Guid stream, Event @event)
        {
            Stream = stream;
            Event = @event;
        }

        public readonly Guid Stream;
        public readonly Event Event;
    }
}