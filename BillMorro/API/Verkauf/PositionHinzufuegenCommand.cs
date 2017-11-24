using System;
using BillMorro.Infrastruktur;

namespace BillMorro.API.Verkauf
{
    public class PositionHinzufuegenCommand : ICommand
    {
        private Guid artikelZigarettenId;
        private decimal testBetrag;

        public PositionHinzufuegenCommand(Guid artikelZigarettenId, decimal testBetrag)
        {
            this.artikelZigarettenId = artikelZigarettenId;
            this.testBetrag = testBetrag;
        }
    }
}