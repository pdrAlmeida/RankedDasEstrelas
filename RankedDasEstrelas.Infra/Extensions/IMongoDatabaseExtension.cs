using MongoDB.Bson;
using MongoDB.Driver;

namespace RankedDasEstrelas.Infra.Extensions
{
    public static class IMongoDatabaseExtension
    {
        public static IMongoCollection<T> CreateCollection<T>(this IMongoDatabase database, string name, CreateCollectionOptions createCollectionOptions = null)
        {
            var filtro = new ListCollectionNamesOptions { Filter = Builders<BsonDocument>.Filter.Eq("name", name) };
            if (!database.ListCollectionNames(filtro).Any()) database.CreateCollection(name, createCollectionOptions);
            return database.GetCollection<T>(name);
        }
    }
}