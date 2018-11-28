using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

/// Create a group
public class CreateBehaviour : Behaviour
{
    public CreateBehaviour(TelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task<Behaviour> ProcessAsync(Update update)
    {
        var msg = update.Message;
        var m = await botClient.SendTextMessageAsync(
            chatId: msg.Chat.Id,
            text: "Hello from CreateBehaviour!"
            );

        return null;
    }
}