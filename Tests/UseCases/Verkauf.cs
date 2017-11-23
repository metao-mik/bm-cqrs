using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.UseCases
{
    [TestClass]
    public class Verkauf_einer_Schachtel_Zigaretten
    {

        private static readonly decimal TestBetrag = 8.14m;

        [TestMethod]
        public void Hinzufuegen_einer_Position()
        {
          Artikel_Zigaretten_wird_ausgewaehlt();
          Betrag_wird_eingegeben(TestBetrag);

          Erwartung__Der_Bon_ist_aktuell();
          Erwartung__Der_Bon_hat_eine_Position();
        }

    private void Erwartung__Der_Bon_ist_aktuell()
    {
      throw new NotImplementedException();
    }

    [TestMethod, Ignore]
        public void Bon_wird_Kassiert()
        {
          Artikel_Zigaretten_wird_ausgewaehlt();
          Betrag_wird_eingegeben(TestBetrag);
          Bon_wird_bar_kassiert();

          Erwartung__Der_Bon_ist_nicht_aktuell();
        }

    private void Erwartung__Der_Bon_ist_nicht_aktuell()
    {
      throw new NotImplementedException();
    }

    private void Bon_wird_bar_kassiert()
    {
      throw new NotImplementedException();
    }

    private void Erwartung__Der_Bon_hat_eine_Position()
    {
      throw new NotImplementedException();
    }

    private void Betrag_wird_eingegeben(object testBetrag)
    {
      throw new NotImplementedException();
    }

    private void Artikel_Zigaretten_wird_ausgewaehlt()
    {
      throw new NotImplementedException();
    }
  }
}
