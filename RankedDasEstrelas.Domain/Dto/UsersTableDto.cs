using RankedDasEstrelas.Domain.Entities;
using System.Collections.Generic;

namespace RankedDasEstrelas.Domain.Dto
{
    public class UsersTableDto
    {
        public UsersTableDto()
        {
        }

        public string Id { get; set; }
        public string NickName { get; set; }

        public string GamesPlayed { get; set; }
        public string Wins { get; set; }
        public string Loses { get; set; }
        public string MVPs { get; set; }
        public string ACEs { get; set; }

        public List<UsersTableDto> GetMaxPropertiesLenght(List<Player> players)
        {
            List<UsersTableDto> usersTable = new List<UsersTableDto>();

            int maxNickNameLength = 0;
            int maxOthersLength = 1;

            for (int i = 0; i < players.Count - 1; i++)
            {
                if (players[i].NickName.Length > maxNickNameLength)
                    maxNickNameLength = players[i].NickName.Length;
            }

            foreach (var player in players)
            {
                UsersTableDto userTable = new();

                userTable.Id = player.Id;
                userTable.NickName = player.NickName;
                userTable.GamesPlayed = player.GamesPlayed.ToString();
                userTable.Wins = player.Wins.ToString();
                userTable.Loses = player.Loses.ToString();
                userTable.MVPs = player.MVPs.ToString();
                userTable.ACEs = player.ACEs.ToString();

                do
                {
                    userTable.NickName = userTable.NickName.Insert(userTable.NickName.Length, " ");
                } while (userTable.NickName.Length <= maxNickNameLength);

                do
                {
                    userTable.GamesPlayed = userTable.GamesPlayed.ToString().Insert(userTable.GamesPlayed.ToString().Length, " ");
                } while (userTable.GamesPlayed.Length < maxOthersLength);

                do
                {
                    userTable.Wins = userTable.Wins.ToString().Insert(userTable.Wins.ToString().Length, " ");
                } while (userTable.Wins.Length < maxOthersLength);

                do
                {
                    userTable.Loses = userTable.Loses.ToString().Insert(userTable.Loses.ToString().Length, " ");
                } while (userTable.Loses.Length < maxOthersLength);

                do
                {
                    userTable.MVPs = userTable.MVPs.ToString().Insert(userTable.MVPs.ToString().Length, " ");
                } while (userTable.MVPs.Length < maxOthersLength);

                do
                {
                    userTable.ACEs = userTable.ACEs.ToString().Insert(userTable.ACEs.ToString().Length, " ");
                } while (userTable.ACEs.Length < maxOthersLength);

                usersTable.Add(userTable);
            }
            return usersTable;
        }
    }
}