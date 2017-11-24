using System;
using BillMorro.API.Verkauf;
using BillMorro.Infrastruktur;

namespace BillMorro.Implementierung
{
    public class VerkaufCommandHandler : CommandHandler
    {
        public VerkaufCommandHandler()
        {
            Register_Command<Position_zu_Bon_hinzufuegen>(Handle);
        }

        private void Handle(Position_zu_Bon_hinzufuegen cmd)
        {
            throw new NotImplementedException();
        }
    }
}