using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteMonitor.Console;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<MonitorSettings>(context.Configuration.GetSection("MonitorSettings"));
        services.AddHostedService<SiteCheckerService>();
    })
    .Build();

await host.RunAsync();