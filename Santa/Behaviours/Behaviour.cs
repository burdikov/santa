using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

public abstract class Behaviour
{
    protected TelegramBotClient BotClient { get; private set; }
    protected SantaDbContext Context { get; private set; }
    protected IServiceProvider ServiceProvider { get; private set; }
    protected Dictionary<int, Func<Update, Task<Behaviour>>> Stages { get; set; }
    protected int Stage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider">This brave idea of requesting a service provider in this method allows
    /// me to keep single instance of Behaviour between requests, while complying with DI demanding to dispose
    /// things at the end of the request. 
    /// Everything is good while this is the only public method on this class.</param>
    /// <param name="update"></param>
    /// <returns></returns>
    public virtual async Task<Behaviour> ProcessAsync(IServiceProvider serviceProvider, Update update)
    {
        ServiceProvider = serviceProvider;
        Context = ServiceProvider.GetService<SantaDbContext>();
        BotClient = ServiceProvider.GetService<TelegramBotClient>();

        return await Stages[Stage](update);
    }

    protected async Task<Person> GetPerson(long id)
    {
        var person = await Context.Persons.FindAsync(id);
        if (person == null)
        {
            person = new Person {Id = id, Name = "kek"};
            Context.Persons.Add(person);
            await Context.SaveChangesAsync();
        }

        return person;
    }
}