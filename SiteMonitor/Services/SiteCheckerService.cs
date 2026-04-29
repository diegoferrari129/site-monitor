using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SiteMonitor.POCO;

namespace SiteMonitor.Console;

public class SiteCheckerService : BackgroundService
{
    private readonly ILogger<SiteCheckerService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly int _intervalSeconds;

    private readonly TelegramSettings? _telegramSettings;
    private bool _wasOffline = false;

    public SiteCheckerService(ILogger<SiteCheckerService> logger, IOptions<MonitorSettings> options, IOptions<TelegramSettings> telegramOptions)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        var settings = options.Value;
        _url = settings.Url;
        _intervalSeconds = settings.CheckIntervalSeconds;

        _telegramSettings = telegramOptions.Value;
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

            bool isOnline = response.IsSuccessStatusCode;
            if (isOnline)
            {
                _logger.LogInformation("[{Time}] {Url} is ONLINE (Status {Code})", DateTime.Now, _url, (int)response.StatusCode);
                if (_wasOffline)
                {
                    _wasOffline = false;
                    await SendTelegramAlert($"[{DateTime.Now}] Site back ONLINE: {_url}");
                }
            }
            else
            {
                _logger.LogWarning("[{Time}] {Url} responded with {Code}", DateTime.Now, _url, (int)response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("[{Time}] {Url} is OFFLINE: {Message}", DateTime.Now, _url, ex.Message);
            if (!_wasOffline)
            {
                _wasOffline = true;
                await SendTelegramAlert($"[{DateTime.Now}] Site OFFLINE: {_url}\nErrore: {ex.Message}");
            }
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

    private async Task SendTelegramAlert(string message)
    {
        if (string.IsNullOrEmpty(_telegramSettings?.BotToken) || string.IsNullOrEmpty(_telegramSettings?.ChatId))
            return;
        try
        {
            string url = $"https://api.telegram.org/bot{_telegramSettings.BotToken}/sendMessage?chat_id={_telegramSettings.ChatId}&text={Uri.EscapeDataString(message)}";   
            await _httpClient.GetAsync(url);
            _logger.LogInformation("Notifica Telegram inviata.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Errore invio Telegram: {Message}", ex.Message);
        }
    }
}