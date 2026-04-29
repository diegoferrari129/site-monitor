using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SiteMonitor.Console;

public class SiteCheckerService : BackgroundService
{
    private readonly ILogger<SiteCheckerService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly int _intervalSeconds;

    public SiteCheckerService(ILogger<SiteCheckerService> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _url = "https://www.google.com";
        _intervalSeconds = 30;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servizio avviato. Controllo {Url} ogni {Interval} secondi", _url, _intervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckWebsite();
            await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
        }
    }

    private async Task CheckWebsite()
    {
        try
        {
            var response = await _httpClient.GetAsync(_url);
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("[{Time}] {Url} is ONLINE (Status {Code})", DateTime.Now, _url, (int)response.StatusCode);
            else
                _logger.LogWarning("[{Time}] {Url} responded with {Code}", DateTime.Now, _url, (int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("[{Time}] {Url} is OFFLINE: {Message}", DateTime.Now, _url, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("[{Time}] Unexpected error for {Url}: {Message}", DateTime.Now, _url, ex.Message);
        }
    }

    public override void Dispose()
    {
        _httpClient.Dispose();
        base.Dispose();
    }
}