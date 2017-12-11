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
  public class ArtikellisteVM : BaseVM
  {
    public ArtikellisteVM(Billmorro.ClientApi.Kasse.KasseQueryApi query){
      _artikelliste = query.GetArtikelliste().Select(a=> new ArtikelVM(a)).ToList();
    }
    private List<ArtikelVM> _artikelliste;
    public List<ArtikelVM> Artikelliste => _artikelliste;

    public event EventHandler<ArtikelId> ArtikelGewaehlt;

    public Action<string> Artikel=> id => {
      var handler = ArtikelGewaehlt;
      if (handler!=null) handler(this,(ArtikelId)id);
    };
  }

  public class ArtikelVM {
    public ArtikelVM(Artikel src){
      _src = src;
    }
    private readonly Artikel _src;
    public string Id => _src.Id.Id.ToString();
    public string Bezeichnung => _src.Bezeichnung;
    public string EinzelpreisText => _src.Einzelpreis.Format();
  }
}