using RankedDasEstrelas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Interfaces.Repositories
{
    public interface IRankingTableRepository
    {
        Task<RankingTable> GetRankingTable();

        Task SaveRankingTable(RankingTable rankingTable);
    }
}