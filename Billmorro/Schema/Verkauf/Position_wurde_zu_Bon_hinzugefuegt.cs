using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Schema.Produkte;

namespace Billmorro.Schema.Verkauf
{
    public class Position_wurde_zu_Bon_hinzugefuegt : Event
    {
        public readonly BonId Bon;
        public readonly ArtikelId Artikel;
        public readonly PositionId Position;
        public readonly decimal Betrag;
        public readonly int Menge;

        public Position_wurde_zu_Bon_hinzugefuegt(BonId bon, PositionId position, ArtikelId artikel, int menge, decimal betrag)
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