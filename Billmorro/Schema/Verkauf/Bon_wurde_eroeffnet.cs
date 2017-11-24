using System;
using Billmorro.Infrastruktur;
using Billmorro.Infrastruktur.Eventsourcing;

namespace Billmorro.Schema.Verkauf
{
    public class Bon_wurde_eroeffnet : Event
    {
        public readonly BonId Bon;

        public Bon_wurde_eroeffnet(BonId bon)
        {
            Bon = bon;
        }

        public override string ToString()
        {
            return $"Bon wurde eröffnet ({Bon}).";
        }
    }
}