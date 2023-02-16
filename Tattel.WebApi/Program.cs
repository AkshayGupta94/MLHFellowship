using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace Tattel.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        // Updated method to follow ASP.Net Core 3.0
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.SetBasePath(basePath);

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                config.AddCommandLine(args);
                config.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://*:5003")
                    .CaptureStartupErrors(true)
                    .UseSetting("detailedErrors", "true")
                    //.UseKestrel()
                    //.UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseSerilog((context, config) =>
                    {
                        config.ReadFrom.Configuration(context.Configuration);
                    });
            });

            return host;
        }
    }
}
