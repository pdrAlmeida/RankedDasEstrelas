using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Interfaces.Services
{
    public interface IRankingTableService
    {
        public Task<string> GetRankingTable();

        public Task BuildRankingTable();
    }
}
