using RankedDasEstrelas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Domain.Interfaces.Repositories
{
    public interface IMatchRepository
    {
        Task<Match> FindMatchAsync(string id);

        Task<Match> FindMatchByURLAsync(string url);

        Task<Match> FindMatchOrDefaultAsync(string id);

        Task SaveMatchAsync(Match match);
    }
}