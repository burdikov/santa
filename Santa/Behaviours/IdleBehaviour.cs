using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

public class IdleBehaviour : Behaviour
{
    public IdleBehaviour(TelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task<Behaviour> ProcessAsync(Update update)
    {
        var typeName = update.Message.Text.Trim('/');
        
        var fullTypeName = $"{typeName}Behaviour";
        var beh = Assembly.GetExecutingAssembly().CreateInstance(
            fullTypeName,
            ignoreCase: true,
            bindingAttr: BindingFlags.Default,
            binder: null,
            args: new object[] { botClient },
            culture: null,
            activationAttributes: null
        ) as Behaviour;

        if (beh == null)
            return null;
        else
            return await beh.ProcessAsync(update);
    }
}