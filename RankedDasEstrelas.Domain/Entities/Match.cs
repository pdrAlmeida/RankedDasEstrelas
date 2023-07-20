using MongoDB.Bson.Serialization.Attributes;

namespace RankedDasEstrelas.Domain.Entities
{
    public class Match
    {
        public Match(
            string url,
            string result,
            string player1,
            string player2,
            string player3,
            string player4,
            string player5)
        {
            Url = url.Trim();
            Id = Url[(Url.LastIndexOf('/') + 1)..].Trim();
            Result = result;
            Player1 = player1;
            Player2 = player2;
            Player3 = player3;
            Player4 = player4;
            Player5 = player5;
        }

        [BsonId]
        public string Id { get; private set; }

        public string Url { get; private set; }
        public string Result { get; private set; }
        public string Player1 { get; private set; }
        public string Player2 { get; private set; }
        public string Player3 { get; private set; }
        public string Player4 { get; private set; }
        public string Player5 { get; private set; }
    }
}