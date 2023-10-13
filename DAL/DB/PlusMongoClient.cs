using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace RepoTask.DAL;

public class PlusMongoClient
{
    public IMongoClient MongoClient;
    public PlusMongoClient(IOptions<PlusMongoDatabaseSettings> databaseSettings)
    {
        MongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
    }
}