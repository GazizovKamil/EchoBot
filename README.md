# Telegram Echo Bot на .NET 7

Этот проект представляет собой простой Telegram echo bot, написанный на C#. Он использует API Telegram Bot для получения и повторной отправки сообщений пользователям, включая различные типы мультимедиа, такие как текст, фотографии, видео, документы и многое другое.

## Возможности
- Повторяет текстовые сообщения.
- Повторно отправляет такие типы мультимедиа, как фотографии, видео, документы, стикеры, голосовые сообщения и видеозаписи (кружочки).
- Корректно обрабатывает неизвестные типы сообщений.

## Требования
Перед запуском убедитесь, что на вашем компьютере установлено следующее:
- .NET 7 SDK
- Пакет NuGet: `Telegram.Бот 18.0.0`

## Инструкции по установке

### 1. Создайте Telegram-бота
1. Откройте Telegram и найдите [BotFather](https://t.me/BotFather).
2. Отправьте команду "/newbot" для создания нового бота.
3. Следуйте инструкциям, чтобы задать имя бота и получить токен API.
4. Сохраните токен для последующего использования в вашем коде.
5. При необходимости настройте своего бота с помощью команды `/mybots` в BotFather.

### 2. Создайте .NET-проект.
1. Создайте новый консольный проект.
2. Установите пакет NuGet "Telegram.Bot":
   ```bash
    dotnet add package Telegram.Bot --version 18.0.0
   ```

### 3. Напишите код бота
Замените содержимое `Program.cs` следующим кодом:

```csharp
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClient("ВАШ_ТЕЛЕГРАМ_БОТ_ТОКЕН");

        var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
        };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token
        );

        Console.WriteLine("Бот запущен. Нажмите любую клавишу для выхода.");
        Console.ReadLine();

        cts.Cancel();
    }

    // Обработка входящих сообщений
    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;

        var message = update.Message;
        var chatId = message.Chat.Id;

        // Обрабатываем текстовые сообщения
        if (message.Text != null)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Вы сказали: {message.Text}",
                cancellationToken: cancellationToken);
        }
        // Обрабатываем фотографии
        else if (message.Photo != null)
        {
            await botClient.SendPhotoAsync(
                chatId: chatId,
                photo: message.Photo[0].FileId,
                caption: "Вы отправили фотографию!",
                cancellationToken: cancellationToken);
        }
        // Обрабатываем документы
        else if (message.Document != null)
        {
            await botClient.SendDocumentAsync(
                chatId: chatId,
                document: message.Document.FileId,
                caption: "Вы отправили документ!",
                cancellationToken: cancellationToken);
        }
        // Обрабатываем видео
        else if (message.Video != null)
        {
            await botClient.SendVideoAsync(
                chatId: chatId,
                video: message.Video.FileId,
                caption: "Вы отправили видео!",
                cancellationToken: cancellationToken);
        }
        // Обрабатываем стикеры
        else if (message.Sticker != null)
        {
            await botClient.SendStickerAsync(
                chatId: chatId,
                sticker: message.Sticker.FileId,
                cancellationToken: cancellationToken);
        }
    }

    // Обработка ошибок
    static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.ToString());
        return Task.CompletedTask;
    }
}

```

### 4. Запустите проект
1. Запустите приложение.
2. Ваш бот начнет работать и прослушивать сообщения от пользователей. Он будет отображать все полученные сообщения, включая различные типы медиафайлов.

### Примечания
- Замените "YOUR_TELEGRAM_BOT_TOKEN" на ваш фактический токен бота, полученный от BotFather.
- Бот может обрабатывать текст, фотографии, видео, документы, стикеры, аудио, голосовые сообщения и видеозаписи. Неподдерживаемые типы сообщений будут возвращать сообщение по умолчанию, указывающее на то, что бот не может их обрабатывать.
