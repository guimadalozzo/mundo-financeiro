using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MundoFinanceiro.Replicacao
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
                        .UseUrls("http://localhost:5006", "https://localhost:5007");
                });
    }
}