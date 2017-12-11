using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using DotNetify;
using Billmorro.Infrastruktur.Reactive;

namespace billmorro_ui
{
  public class TouchpadVM : BaseVM
  {
    public event EventHandler<string> Barcode_hinzufuegen;

    private string _eingabe = "";
    public string DisplayText => _eingabe+"|";

    private bool IstDezimalzahl => _eingabe.Contains(",");
    private int Nachkommastellen => IstDezimalzahl ? (_eingabe.Length - _eingabe.IndexOf(",") - 1) : 0;
    internal int? Menge => MengeMoeglich ? (int?)Int32.Parse(_eingabe) : null;

    public bool BarcodeMoeglich => !IstDezimalzahl && _eingabe.Length>4 && _eingabe.Length<=13;
    public bool MengeMoeglich => !IstDezimalzahl && _eingabe.Length<4 && _eingabe.Length>0;
    public bool BarPreisMoeglich => false;
    public bool KassierenBarMoeglich => false;
    public bool EingabeMoeglich => (!IstDezimalzahl && _eingabe.Length<=13) || Nachkommastellen<2;
    public bool BackspaceMoeglich => _eingabe.Length>0;

    public Action<string> Taste => x => {
      if (IstDezimalzahl){
        if (x==",") return;
        if (Nachkommastellen>=2) return;
      }

      _eingabe += x;
      Changed(nameof(DisplayText));
      Changed(nameof(EingabeMoeglich));
      Changed(nameof(BackspaceMoeglich));
      Changed(nameof(DisplayText));
      Changed(nameof(BarcodeMoeglich));
      Changed(nameof(MengeMoeglich));
      PushUpdates();
    };

    public Action Taste_Backspace => () => {
      if (_eingabe.Length>0) {
        _eingabe = _eingabe.Substring(0,_eingabe.Length-1);
        Changed(nameof(DisplayText));
        Changed(nameof(EingabeMoeglich));
        Changed(nameof(BackspaceMoeglich));
        Changed(nameof(BarcodeMoeglich));
        Changed(nameof(MengeMoeglich));
        PushUpdates();
      }
    };

    public Action Taste_Barcode => () => {
      if (!BarcodeMoeglich) return;
      var handler = Barcode_hinzufuegen;
      if (handler!=null){
        handler(this, _eingabe);
        Reset();
        PushUpdates();
      }
    };

    internal void Reset () {
      _eingabe="";
      Changed(nameof(DisplayText));
      Changed(nameof(BarcodeMoeglich));
      Changed(nameof(MengeMoeglich));
      Changed(nameof(EingabeMoeglich));
      Changed(nameof(BackspaceMoeglich));
    }
  }
}