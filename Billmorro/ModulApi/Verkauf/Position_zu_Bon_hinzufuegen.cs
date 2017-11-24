using System;
using Billmorro.Infrastruktur.CommandSide;
using Billmorro.Schema.Produkte;
using Billmorro.Schema.Verkauf;

namespace Billmorro.ModulApi.Verkauf
{
    public class Position_zu_Bon_hinzufuegen : Command
    {
        public readonly BonId Bon;
        public readonly ArtikelId Artikel;
        public readonly PositionId Position;
        public readonly decimal Betrag;
        public readonly int Menge;
        
        public Position_zu_Bon_hinzufuegen(BonId bon, PositionId position, ArtikelId artikel, int menge, decimal betrag)
        {
            Bon = bon;
            Artikel = artikel;
            Menge = menge;
            Betrag = betrag;
            Position = position;
        }
    }
}