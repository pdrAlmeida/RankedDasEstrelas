using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Infra.DbContext;
using RankedDasEstrelas.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly MongoDBContext dBContext;
        private readonly IMongoSession mongoSession;

        public PlayerRepository(MongoDBContext dBContext, IMongoSession mongoSession)
        {
            this.dBContext = dBContext;
            this.mongoSession = mongoSession;
        }

        public async Task<Player> FindByNickNameAsync(string nickName)
        {
            if (mongoSession.Session != null)
                return await dBContext.Players.Find(mongoSession.Session, x => x.NickName == nickName).FirstOrDefaultAsync();
            return await dBContext.Players.Find(x => x.NickName == nickName).FirstOrDefaultAsync();
        }

        public async Task<Player> FindByIdAsync(string id)
        {
            if (mongoSession.Session != null)
                return await dBContext.Players.Find(mongoSession.Session, x => x.Id == id).FirstOrDefaultAsync();
            return await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<double> GetScoreAsync(string id)
        {
            var user = await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();
            return user.Score != 0 || user.GamesPlayed != 0 ? Math.Round(user.Score / user.GamesPlayed, 1) : 0;
        }

        public async Task<List<Player>> GetAll() => await dBContext.Players.Find(_ => true).SortBy(t => t.NickName).ToListAsync();

        public Task SaveAsync(Player player)
        {
            ReplaceOptions options = new() { IsUpsert = true };
            if (mongoSession.Session != null)
                return dBContext.Players.ReplaceOneAsync(mongoSession.Session, x => x.Id == player.Id, player, options);
            return dBContext.Players.ReplaceOneAsync(x => x.Id == player.Id, player, options);
        }

        public async Task<decimal> GetWinRateAsync(string id)
        {
            var player = await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (player.GamesPlayed == 0)
                return 0;

            return Math.Round(Convert.ToDecimal(player.Wins) / Convert.ToDecimal(player.GamesPlayed) * 100, 1);
        }

        public async Task<decimal> GetLoseRateAsync(string id)
        {
            var player = await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (player.GamesPlayed == 0)
                return 0;

            return Math.Round(Convert.ToDecimal(player.Loses) / Convert.ToDecimal(player.GamesPlayed) * 100, 1);
        }
    }
}