using System;
using Billmorro.Infrastruktur.Eventsourcing;

namespace Billmorro.Infrastruktur
{
    [Serializable]
    public class Error : ApplicationException
    {
        public Error()
        {
        }

        public Error(Event details) : base(details.ToString())
        {
            Details = details;
        }

        public Event Details { get; set; }
    }
}