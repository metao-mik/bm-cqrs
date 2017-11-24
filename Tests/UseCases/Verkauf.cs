using System;
using Billmorro.ClientApi.Kasse;
using Billmorro.ModulApi.Verkauf;
using Billmorro.Implementierung;
using Billmorro.Infrastruktur;
using Billmorro.Infrastruktur.CommandSide;
using Billmorro.Infrastruktur.Implementierung;
using Billmorro.Infrastruktur.Reactive;
using Billmorro.ModulApi.Geraete;
using Billmorro.Schema.Produkte;
using Billmorro.Schema.Verkauf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bon = Billmorro.ClientApi.Kasse.Bon;

namespace Billmorro.Tests.UseCases
{
    [TestClass]
    public class Verkauf_einer_Schachtel_Zigaretten
    {

        private static readonly decimal Betrag_8_Euro_14 = 8.14m;
        private static readonly int Menge_1 = 1;
        private static readonly ArtikelId ArtikelZigarettenId = ArtikelId.Neu;
        private KasseClientApi _clientapi;
        private KasseQueryApi _queryapi;

        [TestMethod]
        public void Hinzufuegen_einer_Position()
        {
          PositionHinzufuegen(ArtikelZigarettenId, Menge_1, Betrag_8_Euro_14);

          //Erwartung__Der_Bon_ist_aktuell();
          Erwartung__Der_Bon_hat_eine_Position();
        }

        private static readonly string Barcode_12345 = "12345";
        private Bon _aktuellerBon;

        [TestMethod]
        public void Hinzufuegen_eines_Barcodes()
        {
            BarcodeHinzufuegen(Barcode_12345);

            //Erwartung__Der_Bon_ist_aktuell();
            Erwartung__Der_Bon_hat_eine_Position();
        }

        private void BarcodeHinzufuegen(string barcode)
        {
            _clientapi.Hinzufuegen_Barcode(barcode);
        }


        [TestInitialize]
        public void Setup()
        {
            var eventstore = new InMemoryEventStore(() => DateTime.UtcNow);

            var geraete = new Geraetemodul();
            _clientapi = new KasseClientApi(
                new ModulApi.Produkte.Produktmodul(),
                geraete,
                    new VerkaufCommandHandler(eventstore,
                        ex => { throw new Exception("Fehler in Testausführung: " + ex.Message, ex); })
                );

            _queryapi = new KasseQueryApi(new VerkaufQuery(eventstore), geraete);

            _aktuellerBon = null;
            _queryapi.AktuellerBon.Subscribe(bon => _aktuellerBon = bon);
        }

        private void PositionHinzufuegen(ArtikelId artikel, int menge, decimal betrag)
        {
            _clientapi.Hinzufuegen_Position(artikel,menge,betrag);
        }

        private void Erwartung__Der_Bon_hat_eine_Position()
        {
            Assert.IsNotNull(_aktuellerBon, "Es wurde ein aktueller Bon erwartet.");
            Assert.AreEqual(1, _aktuellerBon?.Positionen?.Count ?? 0, "Es wurde eine Bonposition erwartet");
        }


        //private void Erwartung__Der_Bon_ist_aktuell()
        //{
            
        //}
    }

}
