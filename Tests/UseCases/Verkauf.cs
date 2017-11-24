using System;
using Billmorro.ModulApi.Verkauf;
using Billmorro.Implementierung;
using Billmorro.Infrastruktur;
using Billmorro.Infrastruktur.CommandSide;
using Billmorro.Infrastruktur.Implementierung;
using Billmorro.Schema.Produkte;
using Billmorro.Schema.Verkauf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Billmorro.Tests.UseCases
{
    [TestClass]
    public class Verkauf_einer_Schachtel_Zigaretten
    {

        private static readonly decimal Betrag_8_Euro_14 = 8.14m;
        private static readonly int Menge_1 = 1;
        private static readonly ArtikelId ArtikelZigarettenId = ArtikelId.Neu;
        private CommandHandler _commandhandler;
        private BonId _bon; // TODO: in Orchestrierung auslagern

        [TestMethod]
        public void Hinzufuegen_einer_Position()
        {
          PositionHinzufuegen(ArtikelZigarettenId, Menge_1, Betrag_8_Euro_14);

          Erwartung__Der_Bon_ist_aktuell();
          Erwartung__Der_Bon_hat_eine_Position();
        }


        [TestInitialize]
        public void Setup()
        {
            var eventstore = new InMemoryEventStore(()=>DateTime.UtcNow);
            _commandhandler = new VerkaufCommandHandler(eventstore,
                ex => { throw new Exception("Fehler in Testausführung: " + ex.Message, ex); });
            _bon = BonId.Neu; // TODO: in Orchestrierung auslagern
        }

        private void PositionHinzufuegen(ArtikelId artikel, int menge, decimal betrag)
        {
          var cmd =  new Position_zu_Bon_hinzufuegen(_bon, PositionId.Neu, artikel, menge, betrag);
          Execute(cmd);
        }

        // Option: 
        // mehrere Commands ausführen als Transaktion, 
        // Meta-Daten an der Transaktion (user Session etc)
        private void Execute(Command cmd)
        {
            _commandhandler.Execute(cmd);
        }

        private void Erwartung__Der_Bon_hat_eine_Position()
        {
            throw new NotImplementedException();
        }


        private void Erwartung__Der_Bon_ist_aktuell()
        {
          throw new NotImplementedException();
        }
    }

}
