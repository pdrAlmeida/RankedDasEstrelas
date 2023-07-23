using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Infra.DbContext;
using System;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MongoDBContext dBContext;

        public MatchRepository(MongoDBContext dBContext) => this.dBContext = dBContext;

        public async Task<Match> FindMatchAsync(string id) 
            => await FindMatchOrDefaultAsync(id) ?? throw new Exception("Partida não cadastrada");

        public async Task<Match> FindMatchByURLAsync(string url) 
            => await dBContext.Matches.Find(x => x.Url == url).FirstOrDefaultAsync();

        public async Task<Match> FindMatchOrDefaultAsync(string id) 
            => await dBContext.Matches.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task SaveMatchAsync(Match match)
            => await dBContext.Matches.ReplaceOneAsync(x => x.Id == match.Id, match, new ReplaceOptions() { IsUpsert = true });
    }
}