using Billmorro.ClientApi.Kasse;
using Billmorro.Implementierung;
using Billmorro.Infrastruktur.Implementierung;
using Billmorro.Infrastruktur.Reactive;
using Billmorro.ModulApi.Geraete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billmorro.Gui.Forms
{
    public partial class KasseForm : Form
    {
        private KasseClientApi _clientapi;
        private KasseQueryApi _queryapi;
        private Bon _aktuellerBon;

        public KasseForm()
        {
            InitializeComponent();
            Startup();
        }



        private void Startup()
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

            //_aktuellerBon = null;
            _queryapi.AktuellerBon.Subscribe(ShowBon);
        }

        private void ShowBon(Bon bon)
        {
            bonIdTextbox.Text = bon.NettoBetrag.ToString();
            AnzahlPosTextbox.Text = bon.Positionen.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var barcode = BarcodeTextBox.Text;
            _clientapi.Hinzufuegen_Barcode(barcode);
        }
    }
}
