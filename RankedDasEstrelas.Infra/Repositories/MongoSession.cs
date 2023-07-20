using MongoDB.Driver;
using RankedDasEstrelas.Infra.DbContext;
using RankedDasEstrelas.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Repositories
{
    public class MongoSession : IMongoSession
    {
        public IClientSessionHandle Session { get; private set; }
        private readonly MongoDBContext _context;

        public MongoSession(MongoDBContext context)
        {
            _context = context;
        }

        public async Task<IMongoSession> StartSessionAsync()
        {
            Session = await _context.Client.StartSessionAsync();
            return this;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Session?.Dispose();
        }
    }
}