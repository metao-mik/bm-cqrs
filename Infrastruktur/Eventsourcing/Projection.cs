using System.Collections.Generic;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public interface Projection<out T>
    {
        T Project(IEnumerable<Event> events);
    }
}