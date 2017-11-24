using System;
using BillMorro.Infrastruktur;

namespace BillMorro.Tests.UseCases
{
    internal class PositionHinzufuegenCommand : ICommand
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