using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASPJWTPractice.Utilities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ASPJWTPractice
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();



        public static void Main(string[] args)
        {
            CurrentDirectoryHelper.SetCurrentDirectory();

            Log.Logger= new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            /*Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Logs\ASPJWTPract\ASPJWTPract log_{Date}.txt")
                .CreateLogger();*/

            try
            {
                Log.Information("Starting web application...");
                CreateWebHostBuilder(args).Build().Run();
                /*BuildWebHost(args).Run();
                return 0;*/
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly. {Error}",ex.Message);
                //return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }          
        }

        /*public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseConfiguration(Configuration)
            .UseSerilog()
            .Build();*/

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseConfiguration(Configuration)
            .UseSerilog();
    }
}
