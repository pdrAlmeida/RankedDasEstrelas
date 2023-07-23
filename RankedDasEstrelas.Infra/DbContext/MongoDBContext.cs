using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Infra.Extensions;

namespace RankedDasEstrelas.Infra.DbContext
{
    public class MongoDBContext
    {
        private IMongoDatabase Database { get; }
        public IMongoCollection<Player> Players { get; }
        public IMongoCollection<Match> Matches { get; }
        public IMongoCollection<RankingTable> RankingTable { get; }

        public MongoDBContext(string connection)
        {
            var mongoUrl = new MongoUrl(connection);
            Database = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
            Players = Database.CreateCollection<Player>("Players");
            Matches = Database.CreateCollection<Match>("Matches");
            RankingTable = Database.CreateCollection<RankingTable>("RankingTable");
        }
    }
}