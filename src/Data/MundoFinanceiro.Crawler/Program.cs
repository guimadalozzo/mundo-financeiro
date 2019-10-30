using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MundoFinanceiro.Crawler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseKestrel()
                        .UseIISIntegration()
                        .UseUrls("http://*:5000", "https://*:5001");
                });
    }
}