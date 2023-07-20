using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace RankDasEstrelas.Bot.DiscordEvents
{
    public static class GuildAvailableEvent
    {
        public static Task Event(DiscordClient client, GuildCreateEventArgs e)
        {
            client.Logger.LogInformation(new EventId(603, "Nova guilda"), $"Guilda {e.Guild.Name} : {e.Guild.MemberCount} Membros.", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}