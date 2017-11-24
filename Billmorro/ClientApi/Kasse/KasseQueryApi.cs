using System;
using System.Collections.Generic;
using Billmorro.Infrastruktur.Reactive;

namespace Billmorro.ClientApi.Kasse
{
    public class KasseQueryApi
    {
        private readonly Subject<Bon> _aktuellerBon = new Subject<Bon>();
        public IObservable<Bon> AktuellerBon => _aktuellerBon;
    }

    public class Bon
    {
        public List<Bonposition> Positionen { get; set; }
        public string Steuersatz1Name { get; set; }
        public decimal Steuersatz1 { get; set; }
        public string Steuersatz2Name { get; set; }
        public decimal Steuersatz2 { get; set; }
        public decimal NettoBetrag { get; set; }
        public decimal BruttoBetrag { get; set; }
    }

    public class Bonposition
    {
        public int Menge { get; set; }
        public string Bezeichnung { get; set; }
        public decimal Positionspreis { get; set; }
        public string Steuersatz { get; set; }
    }

}