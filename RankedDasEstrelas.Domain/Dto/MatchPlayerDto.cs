using RankedDasEstrelas.Domain.Entities;

namespace RankedDasEstrelas.Domain.Dto
{
    public class MatchPlayerDto
    {
        public MatchPlayerDto(Player player, bool win, string rank, double score)
        {
            PlayerId = player.Id;
            if (win) { Win = true; Lose = false; }
            GetPlayerRankOnTheMatch(rank);
            Score = score;
            Player = player;
            Player.UpdateStatistics(Win, Mvp, Ace, Score);
        }

        public string PlayerId { get; private set; }
        public bool Win { get; private set; }
        public bool Lose { get; private set; }
        public bool Ace { get; private set; }
        public bool Mvp { get; private set; }
        public string Rank { get; private set; }

        public double Score { get; private set; }

        public Player Player { get; private set; }

        private void GetPlayerRankOnTheMatch(string rank)
        {
            switch (rank)
            {
                case "MVP":
                    Rank = rank;
                    Mvp = true;
                    Ace = false;
                    break;

                case "ACE":
                    Rank = rank;
                    Ace = true;
                    Mvp = false;
                    break;

                default:
                    Rank = rank[0].ToString();
                    Ace = false;
                    Mvp = false;
                    break;
            }
        }
    }
}