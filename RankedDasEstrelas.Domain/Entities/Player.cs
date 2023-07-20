using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RankedDasEstrelas.Domain.Entities
{
    public class Player
    {
        public Player(string id, string name, string nickName)
        {
            Id = id;
            Name = name;
            NickName = nickName;
        }

        [BsonId]
        public string Id { get; private set; }

        public string Name { get; private set; }
        public string NickName { get; private set; }

        public int GamesPlayed { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int MVPs { get; private set; }
        public int ACEs { get; private set; }
        public double Score { get; private set; }

        public void UpdateStatistics(bool win, bool mvp, bool ace, double score)
        {
            GamesPlayed++;
            Math.Round(Score += score,1);
            if (win)
            {
                Wins++;
                if (mvp) MVPs++;
            }
            else
            {
                Loses++;
                if (ace) ACEs++;
            }
        }
    }
}