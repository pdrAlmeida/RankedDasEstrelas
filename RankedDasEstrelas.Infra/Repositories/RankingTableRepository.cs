using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Infra.DbContext;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class RankingTableRepository : IRankingTableRepository
    {
        private readonly MongoDBContext dBContext;

        public RankingTableRepository(MongoDBContext dBContext) => this.dBContext = dBContext;

        public async Task<RankingTable> GetRankingTable() 
            => await dBContext.RankingTable.Find(r=>r.Table != null).FirstOrDefaultAsync();

        public async Task SaveRankingTable(RankingTable rankingTable) 
            => await dBContext.RankingTable.ReplaceOneAsync(x => x.Id == rankingTable.Id, rankingTable, new ReplaceOptions() { IsUpsert = true });
    }
}