using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteMonitor.Console;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<SiteCheckerService>();
    })
    .Build();

await host.RunAsync();