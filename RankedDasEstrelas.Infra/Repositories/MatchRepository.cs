using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Infra.DbContext;
using RankedDasEstrelas.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MongoDBContext dBContext;
        private readonly IMongoSession mongoSession;

        public MatchRepository(MongoDBContext dBContext, IMongoSession mongoSession)
        {
            this.dBContext = dBContext;
            this.mongoSession = mongoSession;
        }

        public async Task<Match> FindMatchAsync(string id)
        {
            var match = await FindMatchOrDefaultAsync(id);
            if (match == null)
                throw new Exception("Partida não cadastrada");
            return match;
        }

        public async Task<Match> FindMatchByURLAsync(string url)
        {
            if (mongoSession.Session != null)
                return await dBContext.Matches.Find(mongoSession.Session, x => x.Url == url).FirstOrDefaultAsync();
            return await dBContext.Matches.Find(x => x.Url == url).FirstOrDefaultAsync();
        }

        public async Task<Match> FindMatchOrDefaultAsync(string id)
        {
            if (mongoSession.Session != null)
                return await dBContext.Matches.Find(mongoSession.Session, x => x.Id == id).FirstOrDefaultAsync();
            return await dBContext.Matches.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task SaveMatchAsync(Match match)
        {
            ReplaceOptions options = new() { IsUpsert = true };
            if (mongoSession.Session != null)
                return dBContext.Matches.ReplaceOneAsync(mongoSession.Session, x => x.Id == match.Id, match, options);
            return dBContext.Matches.ReplaceOneAsync(x => x.Id == match.Id, match, options);
        }
    }
}