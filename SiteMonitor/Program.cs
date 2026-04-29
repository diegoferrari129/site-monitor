using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteMonitor.Console;
using SiteMonitor.POCO;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<MonitorSettings>(context.Configuration.GetSection("MonitorSettings"));
        services.Configure<TelegramSettings>(context.Configuration.GetSection("TelegramSettings"));
        services.AddHostedService<SiteCheckerService>();
    })
    .Build();

await host.RunAsync();