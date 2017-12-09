using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DotNetify;

namespace billmorro_ui
{
   public class Startup
   {
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddCors();
         services.AddMemoryCache();
         services.AddSignalR();
         services.AddDotNetify();
         services.AddSingleton<Billmorro.Infrastruktur.CommandSide.CommandHandler, Billmorro.Implementierung.VerkaufCommandHandler>();
         services.AddSingleton<Billmorro.Implementierung.VerkaufQuery>();
         services.AddSingleton<System.Action<System.Exception>>(ex=>throw new System.Exception("Fehler",ex));
         services.AddSingleton<System.Func<System.DateTime>>(()=>System.DateTime.UtcNow);
         services.AddSingleton<Billmorro.Infrastruktur.Eventsourcing.EventStore, Billmorro.Infrastruktur.Implementierung.InMemoryEventStore>();
         services.AddSingleton<Billmorro.ModulApi.Geraete.Geraetemodul>();
         services.AddSingleton<Billmorro.ModulApi.Produkte.Produktmodul>();
         services.AddSingleton<Billmorro.ClientApi.Kasse.KasseQueryApi>();
         services.AddSingleton<Billmorro.ClientApi.Kasse.KasseClientApi>();
      }
      public void Configure(IApplicationBuilder app)
      {
         app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
         app.UseWebSockets();
         app.UseSignalR(routes => routes.MapDotNetifyHub());

         app.UseDotNetify();

         app.Run(async (context) =>
         {
             System.Console.WriteLine(context.Request.Path.Value);
              await context.Response.WriteAsync("HelloWorld server");
         });
      }
   }
}