using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Infra.DbContext;
using RankedDasEstrelas.Infra.Interfaces;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class RankingTableRepository : IRankingTableRepository
    {
        private readonly MongoDBContext dBContext;
        private readonly IMongoSession mongoSession;

        public RankingTableRepository(MongoDBContext dBContext, IMongoSession mongoSession)
        {
            this.dBContext = dBContext;
            this.mongoSession = mongoSession;
        }

        public async Task<RankingTable> GetRankingTable() => await dBContext.RankingTable.Find(r=>r.Table != null).FirstOrDefaultAsync();

        public Task SaveRankingTable(RankingTable rankingTable)
        {
            ReplaceOptions options = new() { IsUpsert = true };
            if (GetRankingTable().Result is not null)
            {
                if (mongoSession.Session != null) 
                {
                    return dBContext.RankingTable.ReplaceOneAsync(mongoSession.Session, x => x.Id == rankingTable.Id, rankingTable, options);

                }
                return dBContext.RankingTable.ReplaceOneAsync(x => x.Id == rankingTable.Id, rankingTable, options);
            }
            if (mongoSession.Session != null)
            {
                return dBContext.RankingTable.InsertOneAsync(mongoSession.Session, rankingTable);

            }
            return dBContext.RankingTable.InsertOneAsync(rankingTable);
        }
    }
}