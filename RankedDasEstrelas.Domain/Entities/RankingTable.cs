using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RankedDasEstrelas.Domain.Entities
{
    public class RankingTable
    {
        public RankingTable(string table)
        {
            Id = Guid.NewGuid().ToString();
            Table = table;
        }

        public string Id { get; set; }

        public string Table { get; set; }
    }
}