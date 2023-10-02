using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace RepoTask.DAL;

public class MinusMongoClient
{
    public IMongoClient MongoClient;
    public MinusMongoClient(IOptions<MinusMongoDatabaseSettings> databaseSettings)
    {
        MongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
    }
}