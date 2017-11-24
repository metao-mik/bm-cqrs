using Billmorro.Infrastruktur.Eventsourcing;

namespace Billmorro.Schema.Verkauf
{
    public static class Boneigenschaften
    {
        public static readonly Projection<BonStatus> Status = new SimpleProjection<BonStatus>(
            () => BonStatus.Unbekannt,
            P.On<Bon_wurde_eroeffnet, BonStatus>((s, e) => BonStatus.Offen)//, // Hier darf keine Logik mehr hin, "zu spät"
            //Project.On<Bon_wurde_kassiert>((s, e) => BonStatus.Geschlossen)
        );
    }
}