using System;
using Billmorro.Infrastruktur;

namespace Billmorro.ModulApi.Verkauf
{
    public class Position_zu_Bon_hinzufuegen : Command
    {
        public readonly Guid Bon; // TODO: Guids durch Struct ersetzen
        public readonly Guid Artikel;
        public readonly Guid Position;
        public readonly decimal Betrag;
        public readonly int Menge;
        
        public Position_zu_Bon_hinzufuegen(Guid bon, Guid position, Guid artikel, int menge, decimal betrag)
        {
            Bon = bon;
            Artikel = artikel;
            Menge = menge;
            Betrag = betrag;
            Position = position;
        }
    }
}