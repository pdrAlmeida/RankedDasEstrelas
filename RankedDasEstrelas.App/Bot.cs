using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RankDasEstrelas.Bot.Commands;
using RankDasEstrelas.Bot.DiscordEvents;


namespace RankDasEstrelas.Bot
{
    public class Bot
    {
        private DiscordClient Client { get; set; }
        private CommandsNextExtension CommandsNext { get; set; }

        public Bot(string token, ServiceProvider services)
        {
            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            ModuleCommand(new CommandsNextConfiguration
            {
                EnableDms = false,
                CaseSensitive = false,
                EnableDefaultHelp = false,
                EnableMentionPrefix = true,
                IgnoreExtraArguments = true,
                Services = services,
                StringPrefixes = new string[] { "!" }
            });
        }

        public Task ConectarAsync() => Client.ConnectAsync();

        private void ModuleCommand(CommandsNextConfiguration ccfg)
        {
            CommandsNext = Client.UseCommandsNext(ccfg);
            CommandsNext.CommandExecuted += CommandExecutedEvent.Event;
            CommandsNext.CommandErrored += CommandErroredEvent.EventAsync;
            Client.Ready += ReadyEvent.Event;

            Client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e);
            Client.ClientErrored += ClientErroredEvent.Event;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PaginationDeletion = PaginationDeletion.KeepEmojis,
            });

            CommandsNext.RegisterCommands<BotCommands>();
        }
    }
}