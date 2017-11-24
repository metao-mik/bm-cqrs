using System;
using System.Collections.Generic;
using System.Linq;
using Billmorro.ModulApi.Verkauf;
using Billmorro.Infrastruktur;
using Billmorro.Schema.Verkauf;

namespace Billmorro.Implementierung
{
    public class VerkaufCommandHandler : CommandHandler
    {
        public VerkaufCommandHandler(EventStore eventstore, Action<Exception> on_error) : base(eventstore, on_error)
        {
            Register_Command<Position_zu_Bon_hinzufuegen>(Handle);
        }

        private void Handle(Position_zu_Bon_hinzufuegen cmd, UnitOfWork uow)
        {
            if (Boneigenschaften.Status.Project(uow.History(cmd.Bon)) == BonStatus.Unbekannt)
            {
                uow.AddEvent(cmd.Bon, new Bon_wurde_eroeffnet(cmd.Bon));
            }

            if (Boneigenschaften.Status.Project(uow.History(cmd.Bon)) == BonStatus.Offen)
            {
                uow.AddEvent(cmd.Bon, new Position_wurde_zu_Bon_hinzugefuegt(cmd.Bon, cmd.Position, cmd.Artikel, cmd.Menge, cmd.Betrag));
            }
            else
            {
                // Fachliche Fehler: die Exception "Error" bricht die Transaktion ab, enthält ein Event, dass ggf. statt des Transaktionsewrgebnisses veröffentlicht werden kann (nicht implementiert, s. CommandHandler).
                // Falls der Fehler nicht fatal ist, kann auch das Event einfach so zur Uow hinzugefügt werden.
                throw new Error(new Bon_konnte_nicht_bearbeitet_werden(cmd.Bon, $"Die Position kann nicht hinzugefügt werden, da der Bon im Status '{Boneigenschaften.Status.Project(uow.History(cmd.Bon))}' ist."));
            }
        }
    }

    public static class Boneigenschaften
    {
        public static readonly Projection<BonStatus> Status = new SimpleProjection<BonStatus>(
            () => BonStatus.Unbekannt,
            P.On<Bon_wurde_eroeffnet, BonStatus>((s, e) => BonStatus.Offen)//, // Hier darf keine Logik mehr hin, "zu spät"
            //Project.On<Bon_wurde_kassiert>((s, e) => BonStatus.Geschlossen)
        );
    }

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

    public interface Projection<out T>
    {
        T Project(IEnumerable<Event> events);
    }
}