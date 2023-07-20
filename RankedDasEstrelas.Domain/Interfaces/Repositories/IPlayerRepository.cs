using RankedDasEstrelas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> FindByIdAsync(string id);

        Task<Player> FindByNickNameAsync(string nickName);

        Task<List<Player>> GetAll();

        Task<double> GetScoreAsync(string id);

        Task<decimal> GetWinRateAsync(string id);
        Task<decimal> GetLoseRateAsync(string id);

        Task SaveAsync(Player player);
    }
}