using Billmorro.ClientApi.Kasse;
using Billmorro.Infrastruktur.Reactive;
using Microsoft.AspNetCore.SignalR;

namespace Billmorro.asp
{
    public class KasseHub : Hub
    {
        private readonly KasseClientApi _api;
        private readonly KasseQueryApi _query;

        public KasseHub(ClientApi.Kasse.KasseClientApi api, ClientApi.Kasse.KasseQueryApi query)
        {
            _api = api;
            _query = query;
            _query.AktuellerBon.Subscribe(UpdateBon);
        }

        private void UpdateBon(Bon bon)
        {
            Clients.All.InvokeAsync("UpdateBon", bon);
        }
    }
}