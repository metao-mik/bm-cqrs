using Billmorro.Schema.Produkte;
using System.Collections.Generic;
using System.Linq;

namespace Billmorro.ModulApi.Produkte
{
    public class Produktmodul
    {
      public ArtikelId? Artikel_zu_Barcode_suchen(string barcode) =>
        _artikelstamm.Where(_=>_.Barcode==barcode).Select(_=>(ArtikelId?)_.Id).SingleOrDefault();

      public ArtikelInfo? Artikel(ArtikelId id) =>
        _artikelstamm.Where(_=>_.Id==id).SingleOrDefault();

      public IEnumerable<ArtikelInfo> Gesamtliste () => _artikelstamm;

      private List<ArtikelInfo> _artikelstamm = new List<ArtikelInfo>{
        new ArtikelInfo((ArtikelId)"63272925-62CD-0EFD-A4ED-8698A9C5B297", "12345", "Orbit ohne Zucker, 5er Gebinde", 3.99m, System.Guid.Empty),
        new ArtikelInfo((ArtikelId)"12342925-62CD-0EFD-A4ED-8698A9C5B298", "12378", "Orbit ohne Zucker, Einzelpackung", 0.99m, System.Guid.Empty)
      };
    }

    public struct ArtikelInfo
    {
      public ArtikelInfo(ArtikelId id, string barcode, string bezeichnung, decimal einzelpreis, System.Guid mehrwertsteuersatz){
        Id = id;
        Barcode = barcode;
        Bezeichnung = bezeichnung;
        Einzelpreis = einzelpreis;
        Mehrwertsteuersatz = mehrwertsteuersatz;
      }

      public readonly ArtikelId Id;
      public readonly string Barcode;
      public readonly string Bezeichnung;
      public readonly decimal Einzelpreis;
      public readonly System.Guid Mehrwertsteuersatz;
    }
}