using System;
using BillMorro.Infrastruktur;

namespace BillMorro.API.Verkauf
{
    public class Position_zu_Bon_hinzufuegen : ICommand
    {
        public readonly Guid Bon;
        public readonly Guid Artikel;
        public readonly decimal Betrag;
        public readonly int Menge;
        
        public Position_zu_Bon_hinzufuegen(Guid bon, Guid artikel, int menge, decimal betrag)
        {
            Bon = bon;
            Artikel = artikel;
            Menge = menge;
            Betrag = betrag;
        }
    }
}