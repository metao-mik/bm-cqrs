using Billmorro.Schema.Verkauf;

namespace Billmorro.ModulApi.Geraete
{
    public class Geraetemodul
    {
        private BonId? _bonid;

        public BonId Aktueller_oder_neuer_Bon( /*geraet*/)
        {
            _bonid = _bonid ?? BonId.Neu;
            return _bonid.Value;
        }

        public BonId? Aktueller_Bon => _bonid;
    }
}