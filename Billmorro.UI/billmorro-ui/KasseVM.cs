using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DotNetify;
using Billmorro.Infrastruktur.Reactive;
using Billmorro.Schema.Produkte;

using Billmorro.ClientApi.Kasse;

namespace billmorro_ui
{

  public static class UI
  {
    private static CultureInfo _ci = new CultureInfo("de-DE");
    public static string FormatBetrag(decimal betrag) => betrag.ToString("0.00", _ci);

    public static string Format(this decimal betrag) => FormatBetrag(betrag);
  }

  public class KasseVM : BaseVM
  {
    private readonly Billmorro.ClientApi.Kasse.KasseClientApi _api;
    public KasseVM(Billmorro.ClientApi.Kasse.KasseClientApi api, Billmorro.ClientApi.Kasse.KasseQueryApi query){
      _api = api;
    }

    public override void OnSubVMCreated(BaseVM vm) {
      if (vm is BonVM) InitBonVM((BonVM)vm);
      if (vm is TouchpadVM) InitTouchpadVM((TouchpadVM)vm);
      if (vm is ArtikellisteVM) InitArtikellisteVM((ArtikellisteVM)vm);
    }

    private TouchpadVM _touchpad;

    private void InitBonVM(BonVM vm) {

    }

    private void InitTouchpadVM(TouchpadVM vm) {
      _touchpad = vm;
      vm.Barcode_hinzufuegen += (_,barcode) => _api.Hinzufuegen_Barcode(barcode);
    }

    private void InitArtikellisteVM(ArtikellisteVM vm) {
      vm.ArtikelGewaehlt += (_,artikel) => {
        var menge = System.Math.Max(1, _touchpad?.Menge??0);
        _api.Hinzufuegen_Position(artikel, menge, null);
        _touchpad?.Reset();
      };
    }
  }
}