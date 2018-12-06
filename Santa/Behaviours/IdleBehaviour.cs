using System;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class IdleBehaviour : Behaviour
{
    public override async Task<Behaviour> ProcessAsync(IServiceProvider serviceProvider, Update update)
    {
        var typeName = update.Message.Text.Trim('/');
        if (typeName.ToLower() == "idle") return null;  // Avoid infinite loop.

        var fullTypeName = $"{typeName}Behaviour";

        if (!(Assembly.GetExecutingAssembly().CreateInstance(fullTypeName, ignoreCase: true) is Behaviour beh))
            return null;

        return await beh.ProcessAsync(serviceProvider, update);
    }
}