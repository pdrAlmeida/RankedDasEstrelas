using RankedDasEstrelas.Domain.Dto;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Domain.Interfaces.Services;
using System.Text;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Services
{
    public class RankingTableService : IRankingTableService
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IRankingTableRepository rankingTableRepository;

        public RankingTableService(IPlayerRepository userRepository, IRankingTableRepository rankingTableRepository)
        {
            this.playerRepository = userRepository;
            this.rankingTableRepository = rankingTableRepository;
        }

        public async Task<string> GetRankingTable()
        {
            var str = await rankingTableRepository.GetRankingTable();

            return str == null
                ? new("Não foi possível localizar uma tabela de Ranking, certifique-se de que há mais de uma partida jogada.")
                : new(str.Table);
        }

        public async Task BuildRankingTable()
        {
            StringBuilder str = new();

            str.AppendLine("```\nJOGADOR             JOGOS   WINS    LOSSES  MPVs    ACEs    SCORE");
            str.AppendLine("--------------------------------------------------------------------");

            foreach (var player in await playerRepository.GetAll())
                str.AppendLine($"{player.NickName,-20} {player.GamesPlayed,-7} {player.Wins,-7} {player.Loses,-7} {player.MVPs,-7} {player.ACEs,-7} {await playerRepository.GetScoreAsync(player.Id)}");

            str.AppendLine("```");

            var table = await rankingTableRepository.GetRankingTable();

            if (table == null)
                table = new RankingTable(str.ToString());
            else
                table.Table = str.ToString();

            await rankingTableRepository.SaveRankingTable(table);
        }
    }
}