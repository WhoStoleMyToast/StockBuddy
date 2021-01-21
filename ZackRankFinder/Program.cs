using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace ZackRankFinder
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                          .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                          .MinimumLevel.Override("System", LogEventLevel.Error)
                          .WriteTo.Console()
                          .WriteTo.File("consoleapp.log")
                          .CreateLogger();

            EventId startupEventId = new EventId(1, "startup");

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureServices(services);
                }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var logger = services.GetService<ILogger<Program>>();

                try
                {
                    logger.LogInformation(startupEventId, "Starting...");

                    var myService = services.GetRequiredService<MyApplication>();
                    var result = await myService.Run();

                    logger.LogInformation(result);
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Error Occured");
                }
            }

            return 0;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<MyApplication>();
            services.AddScoped<ISymbolFetcher, SymbolFetcher>();
            services.AddScoped<IRankScraper, RankScraper>();
            services.AddScoped<IRemoteFileSystemContext, FtpRemoteFileSystem>();
            services.AddSingleton<IRemoteFileSystemContext>(x =>
                new FtpRemoteFileSystem(new RemoteSystemSetting()
                {
                    Host = "ftp.nasdaqtrader.com",  /*host ip*/
                    Port = 21,        /*ftp:21, sftp:22*/
                    UserName = "xyz",
                    Password = "abc"
                }));
            services.AddLogging(configure => configure.AddSerilog());
            services.Configure<LoggerFilterOptions>(options => options.AddFilter((x, y) => x.Contains("ZackRankFinder.")));
        }
    }
}
