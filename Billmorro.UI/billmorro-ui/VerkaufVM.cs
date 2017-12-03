using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DotNetify;
using Billmorro.Infrastruktur.Reactive;

using Billmorro.ClientApi.Kasse;

namespace billmorro_ui
{

    public class BonPositionVM {
        public BonPositionVM(Bonposition src){
            _src=src;
        }
        private readonly Bonposition _src;
        public string Bezeichnung => _src.Bezeichnung;
        public string Id => _src.Id.ToString();
        public string Menge => _src.Menge.ToString();
        public string Positionspreis => _src.Positionspreis.ToString("0.00",VerkaufVM.ci);
        public string Steuersatz => _src.Steuersatz;
    }
    public class VerkaufVM : BaseVM
    {

        public static CultureInfo ci = new CultureInfo("de-DE");

        public string NettoBetrag => _aktuellerBon?.NettoBetrag.ToString("0.00",ci)??"";
        public string BruttoBetrag => _aktuellerBon?.BruttoBetrag.ToString("0.00",ci)??"";
        public string PositionenZahl => _aktuellerBon?.Positionen?.Count.ToString()??"";
        public string Steuersatz1 => _aktuellerBon?.Steuersatz1.ToString("0.00",ci)??"";
        public string Steuersatz2 => _aktuellerBon?.Steuersatz2.ToString("0.00",ci)??"";
        public string Steuersatz1Name => _aktuellerBon?.Steuersatz1Name??"";
        public string Steuersatz2Name => _aktuellerBon?.Steuersatz2Name??"";

        public List<BonPositionVM> Positionen => _aktuellerBon?.Positionen.Select(p=>new BonPositionVM(p)).ToList()??new List<BonPositionVM>();
        public Billmorro.ClientApi.Kasse.Bon AktuellerBon => _aktuellerBon;
        private Billmorro.ClientApi.Kasse.Bon _aktuellerBon;
        private KasseClientApi _api;

        public VerkaufVM(Billmorro.ClientApi.Kasse.KasseClientApi api, Billmorro.ClientApi.Kasse.KasseQueryApi query){
            _api = api;

            _unsubscribe = query.AktuellerBon.Subscribe(bon=>{
                _aktuellerBon = bon;
                Changed(nameof(AktuellerBon));
                Changed(nameof(NettoBetrag));
                Changed(nameof(PositionenZahl));
                Changed(nameof(Positionen));
                PushUpdates();
            });
        }

        private IDisposable _unsubscribe;

        public Action<string> Barcode_hinzufuegen => barcode => _api.Hinzufuegen_Barcode(barcode);

    }
}