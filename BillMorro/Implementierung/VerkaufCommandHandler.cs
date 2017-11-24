using System;
using BillMorro.API.Verkauf;
using BillMorro.Infrastruktur;

namespace BillMorro.Implementierung
{
    public class VerkaufCommandHandler : CommandHandler
    {
        public VerkaufCommandHandler(Func<Guid, Guid> tablet_zu_bon_repository)
        {

            Register_Command<PositionHinzufuegenCommand>(Handle);
        }

        private void Handle(PositionHinzufuegenCommand cmd)
        {
            throw new NotImplementedException();
        }
    }
}