﻿using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;

namespace RankDasEstrelas.Bot.DiscordEvents
{
    public static class CommandExecutedEvent
    {
        public static Task Event(CommandsNextExtension cne, CommandExecutionEventArgs e)
        {
            cne.Client.Logger.LogInformation(new EventId(600, "Comando exec"), $"{e.Context.Guild.Name} - {e.Context.User.Id} executou '{e.Command.QualifiedName}'.", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}