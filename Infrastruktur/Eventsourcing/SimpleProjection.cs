using System;
using System.Collections.Generic;
using System.Linq;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public class SimpleProjection<TResult> : Projection<TResult>
    {
        private readonly Func<TResult> _initialValue;
        private readonly Func<TResult, Event, TResult> _reducer;

        public SimpleProjection(Func<TResult> initial_value, params Func<TResult, Event, TResult>[] reducer)
        {
            _initialValue = initial_value;
            _reducer = P.Wrap(reducer);
        }

        public TResult Project(IEnumerable<Event> events)
        {
            return events.Aggregate(_initialValue(), _reducer);
        }
    }
}