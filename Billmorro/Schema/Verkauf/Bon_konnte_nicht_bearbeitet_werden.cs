using System;
using Billmorro.Infrastruktur;
using Billmorro.Infrastruktur.Eventsourcing;

namespace Billmorro.Schema.Verkauf
{
    [Serializable] // Bestandteil einer Exception
    public class Bon_konnte_nicht_bearbeitet_werden : Event
    {
        public readonly BonId Bon;
        public readonly string Reason;

        public Bon_konnte_nicht_bearbeitet_werden(BonId bon, string reason)
        {
            Bon = bon;
            Reason = reason;
        }

        public override string ToString()
        {
            return $"Bon konnte nicht bearbeitet werden: {Reason} ({Bon}).";
        }
    }
}