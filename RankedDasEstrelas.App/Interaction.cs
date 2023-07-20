using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using RankDasEstrelas.Bot.Extensions;

namespace RankDasEstrelas.Bot
{
    public class Interaction
    {
        private readonly CommandContext _ctx;
        private readonly InteractivityExtension _interactivityExtension;

        public Interaction(CommandContext commandContext)
        {
            _ctx = commandContext;
            _interactivityExtension = _ctx.Client.GetInteractivity();
        }

        public async Task<AnswerResult<string>> WaitForReponseAsync(string message)
        {
            var exitEmbed = new DiscordEmbedBuilder();
            exitEmbed.WithDescription(message);
            exitEmbed.WithFooter("Digite 'sair' para fechar.");

            await CommandContextExtension.RespondAsync(_ctx, exitEmbed);

            var wait = await _interactivityExtension.WaitForMessageAsync(x => x.Author.Id == _ctx.User.Id && x.ChannelId == _ctx.Channel.Id);

            if (wait.TimedOut)
                throw new InteractionResponseTimeOutException("Operação cancelada por limite de tempo");

            if (wait.Result.Content.ToLower().Trim() == "sair")
                throw new InteractionCanceledException("Operação cancelada");

            if (string.IsNullOrEmpty(wait.Result.Content.Trim()))
                throw new NullInteractionResponseException("Nenhuma resposta foi fornecida. A operação foi cancelada.");

            return new AnswerResult<string>(false, wait.Result.Content);
        }
    }
    public readonly struct AnswerResult<T>
    {
        public bool TimedOut { get; }
        public T Result { get; }

        public AnswerResult(bool timedOut, T result)
        {
            TimedOut = timedOut;
            Result = result;
        }
    }
}