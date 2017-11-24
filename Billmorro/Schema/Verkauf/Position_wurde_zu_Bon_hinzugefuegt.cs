using System;
using Billmorro.Infrastruktur;

namespace Billmorro.Schema.Verkauf
{
    public class Position_wurde_zu_Bon_hinzugefuegt : Event
    {
        public readonly Guid Bon; // TODO: Guids durch Struct ersetzen
        public readonly Guid Artikel;
        public readonly Guid Position;
        public readonly decimal Betrag;
        public readonly int Menge;

        public Position_wurde_zu_Bon_hinzugefuegt(Guid bon, Guid position, Guid artikel, int menge, decimal betrag)
        {
            Bon = bon;
            Artikel = artikel;
            Menge = menge;
            Betrag = betrag;
            Position = position;
        }

        public override string ToString()
        {
            return $"Position wurde zu Bon hinzugefügt ({Bon}/{Position}: {Artikel}; {Menge}x; {Betrag}).";
        }
    }
}