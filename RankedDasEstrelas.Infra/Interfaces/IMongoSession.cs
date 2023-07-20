using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace RankedDasEstrelas.Infra.Interfaces
{
    public interface IMongoSession : IDisposable
    {
        IClientSessionHandle Session { get; }

        Task<IMongoSession> StartSessionAsync();
    }
}