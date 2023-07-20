using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace RankDasEstrelas.Bot.Extensions
{
    public static class CommandContextExtension
    {
        public static Task<DiscordMessage> RespondAsync(this CommandContext ctx, DiscordEmbedBuilder embed)
          => ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());

        public static Task<DiscordMessage> RespondAsync(this CommandContext ctx, string mensagem, bool? mentionUser = false)
            => mentionUser.Value ? ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}") : ctx.RespondAsync($"{mensagem}");

        public static Task<DiscordMessage> WriteAsync(this CommandContext ctx, string message) => ctx.RespondAsync(message);

        public static Task<DiscordMessage> RespondListAsync(this CommandContext ctx, IEnumerable<string> items, bool? mentionUser = false)
        {
            var list = items.ToList();
            var messageBuilder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
                messageBuilder.AppendLine($"{i + 1}. {list[i]}");

            return ctx.RespondAsync(messageBuilder.ToString(), mentionUser);
        }
    }
}