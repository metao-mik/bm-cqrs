using System;
using System.Linq;

namespace Billmorro.Infrastruktur.Eventsourcing
{
    public static class P
    {
        public static Func<TResult, Event, TResult> On<TEvent, TResult>(Func<TResult, TEvent, TResult> reducer) where TEvent : Event
        {
            return (state, @event) => 
            {
                if (@event.GetType() != typeof(TEvent)) return state;
                return reducer(state, (TEvent) @event);
            };
        }

        internal static Func<TResult, Event, TResult> Wrap<TResult>(Func<TResult, Event, TResult>[] reducers)
        {
            return (s, e) =>
            {
                return reducers.Aggregate(s, (state, reducer) => reducer(state, e));
            };
        }
    }
}