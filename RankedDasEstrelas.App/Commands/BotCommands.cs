using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RankedDasEstrelas.Infra.Interfaces;
using RankedDasEstrelas.Selenium.Interfaces;
using System.Threading.Tasks;
using System;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Domain.Interfaces.Services;
using System.Text;
using System.Linq;
using RankedDasEstrelas.Domain.Validations;
using RankDasEstrelas.Bot.Extensions;

namespace RankDasEstrelas.Bot.Commands
{
    public class BotCommands : BaseCommandModule
    {
        private readonly ISeleniumService seleniumService;
        private readonly IMongoSession mongoSession;
        private readonly IValidations validations;
        private readonly IPlayerRepository playerRepository;
        private readonly IMatchResultService matchResultService;
        private readonly IRankingTableService rankingTableService;

        public BotCommands(ISeleniumService seleniumService, IMongoSession mongoSession, IValidations validations,
            IPlayerRepository playerRepository, IMatchResultService matchResultService, IRankingTableService rankingTableService)
        {
            this.seleniumService = seleniumService;
            this.mongoSession = mongoSession;
            this.validations = validations;
            this.playerRepository = playerRepository;
            this.matchResultService = matchResultService;
            this.rankingTableService = rankingTableService;
        }

        [Command("cadastritoMuchoLouco")]
        [Aliases("ca")]
        [Description("Cadastra o jogador no Banco de Dados")]
        public async Task SignUp(CommandContext commandContext)
        {
            try
            {
                using (await mongoSession.StartSessionAsync())
                {
                    var player = await playerRepository.FindByIdAsync(commandContext.User.Id.ToString());
                    if (player is not null)
                        await commandContext.RespondAsync("você ja está cadastrado.");

                    var nickName = await new Interaction(commandContext).WaitForReponseAsync("Informe seu Nick no Lol");

                    await commandContext.TriggerTypingAsync();

                    if (nickName.Result is not null)
                    {
                        player = new Player(commandContext.User.Id.ToString(), commandContext.User.Username, nickName.Result.ToString());

                        await playerRepository.SaveAsync(player);

                        await commandContext.RespondAsync("o seu cadastro foi realizado com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                await Exception(commandContext, ex);
            }
        }

        [Command("url")]
        [Description("URL da partida no op.GG")]
        public async Task OpenOpGG(CommandContext commandContext)
        {
            try
            {
                using (await mongoSession.StartSessionAsync())
                {
                    var url = new Interaction(commandContext).WaitForReponseAsync("Informe a URL da partida no op.GG").GetAwaiter().GetResult().Result;

                    await commandContext.TriggerTypingAsync();

                    if (url.Contains(';'))
                    {
                        foreach (var item in url.Split(';'))
                            await ProcessMatchData(commandContext, item);

                        await commandContext.WriteAsync("O processamento das URLs enviadas foi concluído.");
                    }
                    else
                        await ProcessMatchData(commandContext, url);
                }
            }
            catch (Exception ex)
            {
                await Exception(commandContext, ex, false);
            }
        }

        [Command("stats")]
        [Description("Exibe os seus dados no Ranking da Flex das Estrelas")]
        public async Task ShowMyStats(CommandContext commandContext)
        {
            try
            {
                using (await mongoSession.StartSessionAsync())
                {
                    await commandContext.TriggerTypingAsync();

                    var player = await playerRepository.FindByIdAsync(commandContext.User.Id.ToString());
                    if (player is null)
                        await commandContext.RespondAsync("você não está cadastrado. Utilize o comando !cadastritoMuchoLouco para se cadastrar.");

                    var str = new StringBuilder();
                    str.AppendLine($"{player.GamesPlayed} Jogos");
                    str.AppendLine($"{player.Wins} | {playerRepository.GetWinRateAsync(player.Id).Result}% Vitórias");
                    str.AppendLine($"{player.Loses} | {playerRepository.GetLoseRateAsync(player.Id).Result}% Derrotas");
                    str.AppendLine($"{player.MVPs} MVPs");
                    str.AppendLine($"{player.ACEs} ACEs");
                    str.AppendLine($"{playerRepository.GetScoreAsync(player.Id).Result} Score");

                    var embed = new DiscordEmbedBuilder();
                    embed.WithThumbnail(commandContext.User.AvatarUrl);
                    embed.WithColor(DiscordColor.CornflowerBlue);
                    embed.WithDescription(str.ToString());
                    embed.Title = player.NickName;

                    await commandContext.RespondAsync(embed);
                }
            }
            catch (Exception ex)
            {
                await Exception(commandContext, ex);
            }
        }

        [Command("ranking")]
        [Aliases("rank")]
        [Description("Exibe o Ranking da Flex das Estrelas")]
        public async Task ShowRanking(CommandContext commandContext)
        {
            try
            {
                using (await mongoSession.StartSessionAsync())
                {
                    await commandContext.TriggerTypingAsync();

                    await commandContext.WriteAsync(await rankingTableService.GetRankingTable());
                }
            }
            catch (Exception ex)
            {
                await Exception(commandContext, ex);
            }
        }

        [Command("rebuildRank")]
        [Aliases("attTable")]
        [Description("Exibe o Ranking da Flex das Estrelas")]
        public async Task BuildRankingTable(CommandContext commandContext)
        {
            try
            {
                using (await mongoSession.StartSessionAsync())
                {
                    await commandContext.TriggerTypingAsync();

                    await rankingTableService.BuildRankingTable();

                    await commandContext.WriteAsync(await rankingTableService.GetRankingTable());
                }
            }
            catch (Exception ex)
            {
                await Exception(commandContext, ex);
            }
        }

        private async Task Exception(CommandContext commandContext, Exception ex, bool? mentionUser = false)
        {
            Console.WriteLine(ex);
            mongoSession.Dispose();

            if (ex.GetType() == typeof(InteractionCanceledException))
                Task.CompletedTask.Wait();

            else if(ex.GetType() == typeof(InteractionResponseTimeOutException))
                await commandContext.RespondAsync("Tempo Esgotado. Não foi possível processar a solicitação.", mentionUser);
            else
                await commandContext.RespondAsync("Erro. Não foi possível processar a solicitação.", mentionUser);
        }

        private async Task ProcessMatchData(CommandContext commandContext, string url)
        {
            var validations = this.validations.ValidateSentMatch(url);
            if (validations.All(p => p.Valid))
            {
                var (match, matchPlayers) = seleniumService.GetMatchData(url);

                await matchResultService.SaveMatchResultAsync(match, matchPlayers);

                await rankingTableService.BuildRankingTable();

                await commandContext.WriteAsync(await rankingTableService.GetRankingTable());
            }
            else
                await commandContext.RespondListAsync(validations.Where(p => !p.Valid).Select(p => p.Message));
        }
    }
}