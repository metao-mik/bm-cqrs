using Billmorro.Schema.Produkte;

namespace Billmorro.ModulApi.Produkte
{
    public class Produktmodul
    {
        public ArtikelId? Artikel_zu_Barcode_suchen(string barcode)
        {
            if (barcode=="12345") return new ArtikelId(new System.Guid("63272925-62CD-0EFD-A4ED-8698A9C5B297"));
            return null;
        }
    }
}