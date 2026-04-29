Servizio background che monitora periodicamente lo stato di un sito web.

- Controllo periodico di un URL configurabile
- Rileva il cambio di stato (online <> offline) - invia una sola notifica telegram per transizione
- Log dettagliato su console con timestamp e stato

---

Tecnologie

- `.NET 10.0`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Configuration.Json`
- `System.Net.Http`

---

Istruzioni

```bash
git clone https://github.com/diegoferrari129/site-monitor.git
cd site-monitor-service
dotnet run

Configurazione:
vai in appsetting.json e inserisci url e intervallo di tempo
"MonitorSettings": {
    "Url": "https://www.google.com",
    "CheckIntervalSeconds": 30
  }

Per testare le notifiche Telegram:
Crea un file appsettings.Secrets.json e inserisci
{
  "TelegramSettings": {
    "BotToken": "123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11",
    "ChatId": "123456789"
  }
}

Come ottenere token e chatId:
1. Su Telegram, cerca @BotFather -> /newbot -> crea un bot, ricevi il token.
2. Scrivi un messaggio al tuo bot, poi visita: https://api.telegram.org/bot<IL_TUO_TOKEN>/getUpdates
nel JSON trovi "chat": { "id": 123456789 }

Se non configuri Telegram, il servizio funziona ugualmente (solo logging).
