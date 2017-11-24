using System;
using Billmorro.Infrastruktur;

namespace Billmorro.Schema.Verkauf
{
    public class Bon_wurde_eroeffnet : Event
    {
        public readonly Guid Bon;

        public Bon_wurde_eroeffnet(Guid bon)
        {
            Bon = bon;
        }

        public override string ToString()
        {
            return $"Bon wurde eröffnet ({Bon}).";
        }
    }
}