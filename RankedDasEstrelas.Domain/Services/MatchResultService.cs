using RankedDasEstrelas.Domain.Dto;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Services
{
    public class MatchResultService : IMatchResultService
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IMatchRepository matchRepository;

        public MatchResultService( IPlayerRepository playerRepository, IMatchRepository matchRepository)
        {
            this.playerRepository = playerRepository;
            this.matchRepository = matchRepository;
        }

        public async Task SaveMatchResultAsync(Match match, List<MatchPlayerDto> matchPlayers)
        {
            foreach (MatchPlayerDto matchPlayer in matchPlayers)
                await playerRepository.SaveAsync(matchPlayer.Player);

            await matchRepository.SaveMatchAsync(match);
        }
    }
}