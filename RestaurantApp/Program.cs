using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Data;
using Serilog;
using Serilog.Formatting.Compact;

namespace RestaurantApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    int zero = 0;
                    int result = 100 / zero;
                    var context = services.GetRequiredService<RestaurantContext>();
                    DataSeed.Seed(context);
                }
                catch (Exception ex)
                {
                    Log.Error("supak");
                    //var logger = services.GetRequiredService<ILogger<Program>>();
                    //logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((hostingContext, loggingConfiguration) => loggingConfiguration
                    .Enrich.FromLogContext()
                    .MinimumLevel.Error()
                    .WriteTo.File(new CompactJsonFormatter(), "log.clef")
                    .WriteTo.Console());
                    webBuilder.UseStartup<Startup>();
                });
    }
}
