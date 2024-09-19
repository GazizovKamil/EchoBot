using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    static async Task Main(string[] args)
    {
        var botToken = "ВАШ_ТЕЛЕГРАМ_БОТ_ТОКЕН"; // Замените на свой токен

        // Создаем клиента бота
        var botClient = new TelegramBotClient(botToken);

        var cancellationToken = new CancellationTokenSource().Token;

        // Настройки получения сообщений
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
        };

        // Запускаем бота
        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        Console.WriteLine("Бот запущен. Нажмите любую клавишу для выхода.");
        Console.ReadKey();
    }

    // Обработка входящих сообщений
    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        var chatId = message.Chat.Id;

        switch (message.Type)
        {
            case MessageType.Text:
                // Пересылка текстового сообщения
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Вы сказали: {message.Text}",
                    cancellationToken: cancellationToken
                );
                break;

            case MessageType.Photo:
                // Пересылка фотографии
                var photo = message.Photo?.LastOrDefault(); // Получаем фото с наибольшим разрешением
                if (photo != null)
                {
                    await botClient.SendPhotoAsync(
                        chatId: chatId,
                        photo: photo.FileId,
                        caption: "Вы отправили фото",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case MessageType.Video:
                // Пересылка видео
                if (message.Video != null)
                {
                    await botClient.SendVideoAsync(
                        chatId: chatId,
                        video: message.Video.FileId,
                        caption: "Вы отправили видео",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case MessageType.Document:
                // Пересылка документа
                if (message.Document != null)
                {
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: message.Document.FileId,
                        caption: "Вы отправили документ",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case MessageType.Sticker:
                // Пересылка стикера
                if (message.Sticker != null)
                {
                    await botClient.SendStickerAsync(
                        chatId: chatId,
                        sticker: message.Sticker.FileId,
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case MessageType.Audio:
                // Пересылка аудиофайла
                if (message.Audio != null)
                {
                    await botClient.SendAudioAsync(
                        chatId: chatId,
                        audio: message.Audio.FileId,
                        caption: "Вы отправили аудиофайл",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case MessageType.Voice:
                // Пересылка голосового сообщения
                if (message.Voice != null)
                {
                    await botClient.SendVoiceAsync(
                        chatId: chatId,
                        voice: message.Voice.FileId,
                        caption: "Вы отправили голосовое сообщение",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case MessageType.VideoNote:
                // Пересылка видео-заметки (кружок)
                if (message.VideoNote != null)
                {
                    await botClient.SendVideoNoteAsync(
                        chatId: chatId,
                        videoNote: message.VideoNote.FileId,
                        cancellationToken: cancellationToken
                    );
                }
                break;

            default:
                // Пересылка неизвестного типа сообщения
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Извините, я не знаю, как обработать этот тип сообщения.",
                    cancellationToken: cancellationToken
                );
                break;
        }
    }

    // Обработка ошибок
    static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}
