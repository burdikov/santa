using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

/// Create a group
public class CreateBehaviour : Behaviour
{
    private static readonly Regex GroupNameRegex = new Regex("^[А-Яа-яA-Za-z0-9 !?.,]{1,50}$");
    private static readonly Regex PassRegex = new Regex("^[a-zA-Z0-9]{1,8}$");
    private string _groupName;
    private string _pass;

    public CreateBehaviour()
    {
        Stages = new Dictionary<int, Func<Update, Task<Behaviour>>>
        {
            {0, AskName},
            {1, CheckName},
            {2, AskPass},
            {3, CheckPass},
            {4, CreateGroup}
        };
    }

    private async Task<Behaviour> CheckPass(Update update)
    {
        var msgText = update.Message.Text;
        if (PassRegex.IsMatch(msgText))
        {
            _pass = msgText;
            Stage++;
            return await ProcessAsync(ServiceProvider, update);
        }

        await BotClient.SendTextMessageAsync(
            update.Message.Chat.Id,
            "Пароль не подходит. Попробуйте другой."
        );
        return this;
    }

    private async Task<Behaviour> CheckName(Update update)
    {
        var msgText = update.Message.Text;
        if (!string.IsNullOrWhiteSpace(msgText) && GroupNameRegex.IsMatch(msgText))
        {
            _groupName = msgText;
            Stage++;
            return await ProcessAsync(ServiceProvider, update);
        }

        await BotClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Имя не подходит. Попробуйте другое."
        );
        return this;
    }

    private async Task<Behaviour> AskPass(Update update)
    {
        await BotClient.SendTextMessageAsync(
            update.Message.Chat.Id,
            "Придумайте пароль. Длина от 1 до 8 символов, только латиница и цифры."
        );
        Stage++;

        return this;
    }

    private async Task<Behaviour> AskName(Update update)
    {
        var m = await BotClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Пожалуйста, пришлите мне название для группы длиной не более 50 символов. " +
                  "Используйте буквы, цифры, пробелы, точки, запятые и восклицательные и вопросительные знаки."
        );
        Stage++;

        return this;
    }

    private async Task<Behaviour> CreateGroup(Update update)
    {
        var person = await GetPerson(update.Message.From.Id);
        var group = new Group
        {
            Joinable = true,
            Name = _groupName,
            Pass = _pass,
            Owner = person
        };

        Context.Groups.Add(group);
        await Context.SaveChangesAsync();

        await BotClient.SendTextMessageAsync(
            update.Message.From.Id,
            $"Группа успешно создана. Используйте ID: {group.Id} для присоединения."
        );

        return null;
    }
}