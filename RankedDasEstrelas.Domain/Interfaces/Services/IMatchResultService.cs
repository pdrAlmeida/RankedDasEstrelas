using RankedDasEstrelas.Domain.Dto;
using RankedDasEstrelas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Interfaces.Services
{
    public interface IMatchResultService
    {
        Task SaveMatchResultAsync(Match match, List<MatchPlayerDto> matchPlayers);
    }
}