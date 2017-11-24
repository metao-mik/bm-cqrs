using System.Collections.Generic;
using System.Linq;
using Billmorro.Infrastruktur.Eventsourcing;
using Billmorro.Schema.Produkte;

namespace Billmorro.Schema.Verkauf
{
    public class Bonposition
    {
        public PositionId Id { get; set; }
        public int Menge { get; set; }
        public ArtikelId Artikel { get; set; }
        public decimal Positionspreis { get; set; }
    }

    public static class Bon
    {
        public static readonly Projection<BonStatus> Status = new SimpleProjection<BonStatus>(
            () => BonStatus.Unbekannt,
            P.On<Bon_wurde_eroeffnet, BonStatus>((s, e) => BonStatus.Offen)//, // Hier darf keine Logik mehr hin, "zu spät"
            //Project.On<Bon_wurde_kassiert>((s, e) => BonStatus.Geschlossen)
        );

        public static readonly Projection<List<Bonposition>> Positionen = new SimpleProjection<List<Bonposition>>(
            ()=>new List<Bonposition>(),
            P.On<Position_wurde_zu_Bon_hinzugefuegt, List<Bonposition>>((s, e) =>
            {
                return s.Concat(new[]
                {
                    new Bonposition
                    {
                        Id = e.Position,
                        Menge = e.Menge,
                        Artikel = e.Artikel,
                        Positionspreis = e.Betrag
                    }
                }).ToList();
            })
            );
    }
}