using System;
using System.Collections.Generic;
using System.Linq;

namespace Billmorro.Infrastruktur
{
    public class EventSourced_UnitOfWork : UnitOfWork
    {
        private readonly EventStore _eventstore;
        private readonly List<UncommittedEvent> _uncommittedEvents = new List<UncommittedEvent>();

        public EventSourced_UnitOfWork(EventStore eventstore)
        {
            _eventstore = eventstore;
        }

        public IEnumerable<Event> History(Guid stream)
        {
            return
                _eventstore
                    .History(stream).Select(_ => _.Event)
                    .Concat(_uncommittedEvents.Where(_ => _.Stream == stream).Select(_ => _.Event));
        }

        public void AddEvent(Guid stream, params Event[] events)
        {
            _uncommittedEvents.AddRange(events.Select(e=>new UncommittedEvent(stream,e)));
        }

        public void Commit()
        {
            _eventstore.Publish(_uncommittedEvents);
        }
    }
}