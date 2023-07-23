using MongoDB.Driver;
using RankedDasEstrelas.Domain.Entities;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Infra.DbContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly MongoDBContext dBContext;

        public PlayerRepository(MongoDBContext dBContext) => this.dBContext = dBContext;

        public async Task<Player> FindByNickNameAsync(string nickName) 
            => await dBContext.Players.Find(x => x.NickName == nickName).FirstOrDefaultAsync();

        public async Task<Player> FindByIdAsync(string id) 
            => await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<double> GetScoreAsync(string id)
        {
            var user = await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();
            return user.Score != 0 || user.GamesPlayed != 0 ? Math.Round(user.Score / user.GamesPlayed, 1) : 0;
        }

        public async Task<List<Player>> GetAll() 
            => await dBContext.Players.Find(_ => true).SortBy(t => t.NickName).ToListAsync();

        public async Task SaveAsync(Player player) 
            => await dBContext.Players.ReplaceOneAsync(x => x.Id == player.Id, player, new ReplaceOptions() { IsUpsert = true });

        public async Task<decimal> GetWinRateAsync(string id)
        {
            var player = await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();

            return player.GamesPlayed != 0 ? Math.Round(Convert.ToDecimal(player.Wins) / Convert.ToDecimal(player.GamesPlayed) * 100, 1) : 0;
        }

        public async Task<decimal> GetLoseRateAsync(string id)
        {
            var player = await dBContext.Players.Find(x => x.Id == id).FirstOrDefaultAsync();

            return player.GamesPlayed != 0 ? Math.Round(Convert.ToDecimal(player.Loses) / Convert.ToDecimal(player.GamesPlayed) * 100, 1) : 0;
        }
    }
}