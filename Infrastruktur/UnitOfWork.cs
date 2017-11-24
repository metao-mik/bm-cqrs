using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur
{
    public interface UnitOfWork
    {
        IEnumerable<Event> History(Guid stream);
        void AddEvent(Guid stream, params Event[] events);
    }
}