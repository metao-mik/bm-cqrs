﻿using Microsoft.AspNetCore.Builder;
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
      }
      public void Configure(IApplicationBuilder app)
      {
         app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());   
         app.UseWebSockets();
         app.UseSignalR(routes => routes.MapDotNetifyHub());
         app.UseDotNetify();
         app.Run(async (context) =>
         {
            await context.Response.WriteAsync("HelloWorld server");
         });
      }
   }
}