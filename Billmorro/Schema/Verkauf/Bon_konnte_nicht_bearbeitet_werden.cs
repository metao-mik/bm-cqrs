using System;
using Billmorro.Infrastruktur;

namespace Billmorro.Schema.Verkauf
{
    [Serializable] // Bestandteil einer Exception
    public class Bon_konnte_nicht_bearbeitet_werden : Event
    {
        public readonly Guid Bon;
        public readonly string Reason;

        public Bon_konnte_nicht_bearbeitet_werden(Guid bon, string reason)
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