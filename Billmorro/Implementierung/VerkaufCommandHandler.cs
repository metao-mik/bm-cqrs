using System;
using Billmorro.ModulApi.Verkauf;
using Billmorro.Infrastruktur;
using Billmorro.Infrastruktur.CommandSide;
using Billmorro.Infrastruktur.Eventsourcing;
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
            if (Bon.Status.Project(uow.History(cmd.Bon)) == BonStatus.Unbekannt)
            {
                uow.AddEvent(cmd.Bon, new Bon_wurde_eroeffnet(cmd.Bon));
            }

            if (Bon.Status.Project(uow.History(cmd.Bon)) == BonStatus.Offen)
            {
                uow.AddEvent(cmd.Bon, new Position_wurde_zu_Bon_hinzugefuegt(cmd.Bon, cmd.Position, cmd.Artikel, cmd.Menge, cmd.Betrag));
            }
            else
            {
                // Fachliche Fehler: die Exception "Error" bricht die Transaktion ab, enthält ein Event, dass ggf. statt des Transaktionsewrgebnisses veröffentlicht werden kann (nicht implementiert, s. CommandHandler).
                // Falls der Fehler nicht fatal ist, kann auch das Event einfach so zur Uow hinzugefügt werden.
                throw new Error(new Bon_konnte_nicht_bearbeitet_werden(cmd.Bon, $"Die Position kann nicht hinzugefügt werden, da der Bon im Status '{Bon.Status.Project(uow.History(cmd.Bon))}' ist."));
            }
        }
    }
}