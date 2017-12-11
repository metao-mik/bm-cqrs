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
  public class BonVM : BaseVM
  {
    private readonly IDisposable _unsubscribe;

    public BonVM(Billmorro.ClientApi.Kasse.KasseQueryApi query)
    {
      _unsubscribe = query.AktuellerBon.Subscribe(bon=>{
          _aktuellerBon = bon;
          Changed(nameof(AktuellerBon));
          Changed(nameof(NettoBetrag));
          Changed(nameof(BruttoBetrag));
          Changed(nameof(PositionenZahl));
          Changed(nameof(Positionen));
          Changed(nameof(Steuersatz1));
          Changed(nameof(Steuersatz2));
          Changed(nameof(Steuersatz1Name));
          Changed(nameof(Steuersatz2Name));
          PushUpdates();
        });
    }

    public Billmorro.ClientApi.Kasse.Bon AktuellerBon => _aktuellerBon;
    private Billmorro.ClientApi.Kasse.Bon _aktuellerBon;

    public string NettoBetrag => _aktuellerBon?.NettoBetrag.Format()??"";
    public string BruttoBetrag => _aktuellerBon?.BruttoBetrag.Format()??"";
    public string PositionenZahl => _aktuellerBon?.Positionen?.Count.ToString()??"";
    public string Steuersatz1 => _aktuellerBon?.Steuersatz1.Format()??"";
    public string Steuersatz2 => _aktuellerBon?.Steuersatz2.Format()??"";
    public string Steuersatz1Name => _aktuellerBon?.Steuersatz1Name??"";
    public string Steuersatz2Name => _aktuellerBon?.Steuersatz2Name??"";
    public List<BonPositionVM> Positionen => _aktuellerBon?.Positionen.Select(p=>new BonPositionVM(p)).ToList()??new List<BonPositionVM>();
  }

  public class BonPositionVM {
    public BonPositionVM(Bonposition src){
      _src=src;
    }
    private readonly Bonposition _src;
    public string Bezeichnung => _src.Bezeichnung;
    public string Id => _src.Id.ToString();
    public string Menge => _src.Menge.ToString();
    public string Positionspreis => _src.Positionspreis.Format();
    public string Steuersatz => _src.Steuersatz;
  }
}