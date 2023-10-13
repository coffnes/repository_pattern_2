using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace RepoTask.DAL;

public class ZeroMongoClient
{
    public IMongoClient MongoClient;
    public ZeroMongoClient(IOptions<ZeroMongoDatabaseSettings> databaseSettings)
    {
        MongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
    }
}