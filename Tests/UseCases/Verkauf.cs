using System;
using BillMorro.Infrastruktur;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillMorro.Tests.UseCases
{
    [TestClass]
    public class Verkauf_einer_Schachtel_Zigaretten
    {

        private static readonly decimal TestBetrag = 8.14m;
        private static readonly Guid ArtikelZigarettenId = Guid.NewGuid();
        private CommandHandler _commandhandler;

        [TestMethod]
        public void Hinzufuegen_einer_Position()
        {
          PositionHinzufuegen(ArtikelZigarettenId, TestBetrag);

          Erwartung__Der_Bon_ist_aktuell();
          Erwartung__Der_Bon_hat_eine_Position();
        }


        [TestInitialize]
        public void Setup()
        {
            var bon = Guid.NewGuid();
            _commandhandler = new VerkaufCommandHandler(_=>bon);
        }

        private void PositionHinzufuegen(Guid ArtikelZigarettenId, decimal testBetrag)
        {
          var cmd =  new PositionHinzufuegenCommand(ArtikelZigarettenId, testBetrag);
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
