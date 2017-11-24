using System;
using System.Collections.Generic;
using Billmorro.Infrastruktur.Eventsourcing;

namespace Billmorro.Infrastruktur.CommandSide
{
    public interface UnitOfWork
    {
        IEnumerable<Event> History(Guid stream);
        void AddEvent(Guid stream, params Event[] events);
    }
}