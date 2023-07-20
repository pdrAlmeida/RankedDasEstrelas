using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Infra.Extensions;

namespace RankedDasEstrelas.Infra.DbContext
{
    public class MongoDBContext
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<Player> Players { get; }
        public IMongoCollection<Match> Matches { get; }
        public IMongoCollection<RankingTable> RankingTable { get; }

        public MongoDBContext(string connection)
        {
            var mongoUrl = new MongoUrl(connection);
            Client = new MongoClient(mongoUrl);
            Database = Client.GetDatabase(mongoUrl.DatabaseName);
            Players = Database.CreateCollection<Player>("Players");
            Matches = Database.CreateCollection<Match>("Matches");
            RankingTable = Database.CreateCollection<RankingTable>("RankingTable");
        }
    }
}