Servizio background che monitora periodicamente lo stato di un sito web.

- Controllo periodico di un URL configurabile
- Rileva il cambio di stato (online <> offline) e invia una sola notifica telegram per transizione
- Log dettagliato su console con timestamp e stato

| Console Log | Telegram Notification |
|-------------|----------------------|
| <img width="886" height="426" alt="Screenshot 2026-04-30 000135" src="https://github.com/user-attachments/assets/dd6bb9ab-9b19-4bff-aa68-e32caad5fa9c" /> | <img width="1448" height="167" alt="Screenshot 2026-04-30 000235" src="https://github.com/user-attachments/assets/5eb917c7-610d-4738-ae71-219d9f3bb352" /> |

---

Istruzioni

```bash
git clone https://github.com/diegoferrari129/site-monitor.git
cd site-monitor
dotnet run

Configurazione:
vai in appsetting.json e inserisci url e intervallo di tempo
"MonitorSettings": {
    "Url": "https://www.google.com",
    "CheckIntervalSeconds": 30
  }

Per testare le notifiche Telegram:

Come ottenere token e chatId:
1. Su Telegram, cerca @BotFather -> /newbot -> crea un bot, ricevi il token.
2. Scrivi un messaggio al tuo bot, poi visita: https://api.telegram.org/bot<IL_TUO_TOKEN>/getUpdates nel JSON trovi "chat": { "id": 123456789 }

Crea un file appsettings.Secrets.json e inserisci
{
  "TelegramSettings": {
    "BotToken": "il token ricevuto",
    "ChatId": "l'id ricevuto"
  }
}

Se non configuri Telegram, il servizio funziona ugualmente (solo logging).
```

---

Tecnologie

- `.NET 10.0`
- `Microsoft.Extensions.Hosting`
- `System.Net.Http`

---

## Autore
[![GitHub](https://img.shields.io/badge/GitHub-diegoferrari129-181717?logo=github)](https://github.com/diegoferrari129)
