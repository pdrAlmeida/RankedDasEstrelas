using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace RankDasEstrelas.Bot.DiscordEvents
{
    public static class ReadyEvent
    {
        public static Task Event(DiscordClient client, ReadyEventArgs e)
        {
            client.Logger.Log(LogLevel.Information, "Cliente está pronto.", DateTime.Now);
            client.UpdateStatusAsync(new DiscordActivity($"Rank das Estrelas", ActivityType.Playing), UserStatus.Online);
            return Task.CompletedTask;
        }
    }
}