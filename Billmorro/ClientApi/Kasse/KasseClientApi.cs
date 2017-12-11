using Billmorro.Implementierung;
using Billmorro.Infrastruktur.CommandSide;
using Billmorro.ModulApi.Geraete;
using Billmorro.ModulApi.Produkte;
using Billmorro.ModulApi.Verkauf;
using Billmorro.Schema.Produkte;
using Billmorro.Schema.Verkauf;

namespace Billmorro.ClientApi.Kasse
{
    public class KasseClientApi
    {
        private readonly Produktmodul _produkte;
        private readonly Geraetemodul _geraet;
        private readonly CommandHandler _verkauf;

        public KasseClientApi(Produktmodul produkte, Geraetemodul geraet, CommandHandler verkauf)
        {
            _produkte = produkte;
            _geraet = geraet;
            _verkauf = verkauf;
        }

        public void Hinzufuegen_Barcode(string barcode)
        {
            var artikelId_opt = _produkte.Artikel_zu_Barcode_suchen(barcode);

            if (artikelId_opt.HasValue)
            {
                Hinzufuegen_Position(artikelId_opt.Value, 1, null);
            }
            else
            {
                // Neuen Artikel eingeben?
            }
        }

        public void Hinzufuegen_Position(ArtikelId artikel, int menge, decimal? preis)
        {
            var bonId = _geraet.Aktueller_oder_neuer_Bon();
            var betrag = preis ?? _produkte.Artikel(artikel)?.Einzelpreis ?? 0m;
            var cmd = new Position_zu_Bon_hinzufuegen(bonId, PositionId.Neu, artikel, menge, betrag);
            _verkauf.Execute(cmd);
        }
    }
}