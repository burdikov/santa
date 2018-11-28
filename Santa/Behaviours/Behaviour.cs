using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

public abstract class Behaviour
{
    protected TelegramBotClient botClient;
    protected int Stage { get; private set; }

    public Behaviour(TelegramBotClient botClient)
    {
        this.botClient = botClient;
    }

    public abstract Task<Behaviour> ProcessAsync(Update update);
}