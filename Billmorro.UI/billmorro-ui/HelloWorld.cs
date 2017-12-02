using System;
using System.Threading;
using DotNetify;

namespace billmorro_ui
{
    public class HelloWorld : BaseVM
    {
        private Timer _timer;
        public string Greetings => "Cheerio";
        public DateTime ServerTime => DateTime.Now;

        public HelloWorld()
        {
            _timer = new Timer(state =>
            {
                Changed(nameof(ServerTime));
                PushUpdates();
            }, null, 0, 1000);
        }
        public override void Dispose() => _timer.Dispose();
    }
}