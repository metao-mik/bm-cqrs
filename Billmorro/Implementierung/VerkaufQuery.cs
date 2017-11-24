using System;
using System.Collections.Generic;
using System.Linq;
using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Infrastruktur.Reactive;
using Billmorro.Schema.Produkte;
using Billmorro.Schema.Verkauf;

namespace Billmorro.Implementierung
{
    public class VerkaufQuery
    {
        private readonly EventStore _eventstore;
        private readonly Subject<bool> _changes = new Subject<bool>();
        public IObservable<bool> Changes => _changes;

        public VerkaufQuery(EventStore eventstore)
        {
            _eventstore = eventstore;
            eventstore.Eventsets.Subscribe(eventset =>
            {
                _changes.Next(true); // Parameter ist bedeutungslos. Es geht leider nicht ohne.
            });
        }

        public BonReadmodel GetBon(BonId bon)
        {
            var positionen = Bon.Positionen.Project(_eventstore.History(bon).Select(_=>_.Event));
            return new BonReadmodel
            {
                Positionen = positionen,
                NettoBetrag= positionen.Sum(_=>_.Menge * _.Positionspreis),
                BruttoBetrag=0m
            };
        }
    }

    public class BonReadmodel
    {
        public List<Bonposition> Positionen { get; set; }
        public decimal NettoBetrag { get; set; }
        public decimal BruttoBetrag { get; set; }
    }    
}