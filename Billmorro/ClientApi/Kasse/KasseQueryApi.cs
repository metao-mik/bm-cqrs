using System;
using System.Collections.Generic;
using System.Linq;
using Billmorro.Implementierung;
using Billmorro.Infrastruktur.Reactive;
using Billmorro.ModulApi.Geraete;
using Billmorro.Schema.Verkauf;

namespace Billmorro.ClientApi.Kasse
{
    public class KasseQueryApi
    {
        public KasseQueryApi(VerkaufQuery verkaufreadmodel, Geraetemodul geraete)
        {
            verkaufreadmodel.Changes.Subscribe(_ =>
            {
                var bonid = geraete.Aktueller_Bon;
                var bon =
                    bonid.HasValue
                        ? Map(verkaufreadmodel.GetBon(bonid.Value))
                        : LeererBon ;
                _aktuellerBon.Next(bon);
            });
        }

        private Bonposition Map(Billmorro.Schema.Verkauf.Bonposition src)
        {
            return new Bonposition
            {
                Id = src.Id,
                Bezeichnung = src.Artikel.ToString(),
                Menge = src.Menge,
                Positionspreis = src.Positionspreis,
                Steuersatz = "19%"
            };
        }

        private Bon Map(BonReadmodel src)
        {
            return new Bon
            {
                NettoBetrag = src.NettoBetrag,
                BruttoBetrag = src.BruttoBetrag,
                Positionen = src.Positionen.Select(Map).ToList(),
                Steuersatz1Name="19%",
                Steuersatz2Name="7%"
            };
        }

        private Bon LeererBon => new Bon{ Positionen = new List<Bonposition>()};

        private readonly Subject<Bon> _aktuellerBon = new Subject<Bon>(true);
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
        public Guid Id {get;set;}
        public int Menge { get; set; }
        public string Bezeichnung { get; set; }
        public decimal Positionspreis { get; set; }
        public string Steuersatz { get; set; }
    }

}