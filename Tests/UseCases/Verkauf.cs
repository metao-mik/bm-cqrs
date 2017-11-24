using System;
using BillMorro.API.Verkauf;
using BillMorro.Implementierung;
using BillMorro.Infrastruktur;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillMorro.Tests.UseCases
{
    [TestClass]
    public class Verkauf_einer_Schachtel_Zigaretten
    {

        private static readonly decimal TestBetrag = 8.14m;
        private static readonly int TestMenge = 1;
        private static readonly Guid ArtikelZigarettenId = Guid.NewGuid();
        private CommandHandler _commandhandler;
        private Guid _bon; // TODO: in Orchestrierung auslagern

        [TestMethod]
        public void Hinzufuegen_einer_Position()
        {
          PositionHinzufuegen(ArtikelZigarettenId, TestMenge, TestBetrag);

          Erwartung__Der_Bon_ist_aktuell();
          Erwartung__Der_Bon_hat_eine_Position();
        }


        [TestInitialize]
        public void Setup()
        {
            _commandhandler = new VerkaufCommandHandler();
            _bon = Guid.NewGuid(); // TODO: in Orchestrierung auslagern
        }

        private void PositionHinzufuegen(Guid artikel, int menge, decimal betrag)
        {
          var cmd =  new Position_zu_Bon_hinzufuegen(_bon, artikel, menge, betrag);
          Execute(cmd);
        }

        // Option: 
        // mehrere Commands ausführen als Transaktion, 
        // Meta-Daten an der Transaktion (user Session etc)
        private void Execute(ICommand cmd)
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
