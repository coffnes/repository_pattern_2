using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RepoTask.DAL.Models;

namespace RepoTask.DAL.Repositories;

public class PlusMongoRepository : IPlusRepository<string>
{
    private readonly IMongoCollection<TemperatureEntity<string>> _plusTemperatureCollection;
    private readonly PlusMongoClient _mongoClient;
    public PlusMongoRepository(PlusMongoClient mongoClient, IOptions<PlusMongoDatabaseSettings> weatherDatabaseSettings)
    {
        _mongoClient = mongoClient;
        var mongoDatabse = _mongoClient.MongoClient.GetDatabase(weatherDatabaseSettings.Value.DatabaseName);
        _plusTemperatureCollection = mongoDatabse.GetCollection<TemperatureEntity<string>>(weatherDatabaseSettings.Value.PlusTemperatureCollectionName);
    }
    public async Task AddAsync(TemperatureEntity<string> entity)
    {
        await _plusTemperatureCollection.InsertOneAsync(entity);
        // var session = _mongoClient.MongoClient.StartSession();
        // session.StartTransaction();
        // try
        // {
        //     await _plusTemperatureCollection.InsertOneAsync(session, entity);
        //     session.CommitTransaction();
        // }
        // catch(Exception e)
        // {
        //     //логирование
        //     Console.WriteLine(e);
        //     session.AbortTransaction();
        // }
    }

    public async Task AddChunkAsync(IList<TemperatureEntity<string>> entities)
    {
        await _plusTemperatureCollection.InsertManyAsync(entities);
    }
    public IList<TemperatureEntity<string>> GetByCity(string? city)
    {
        if(city == "" || city == "None") {
            return _plusTemperatureCollection.Aggregate().ToList();
        }
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .Eq(m => m.City, city);
        var result = _plusTemperatureCollection.Aggregate()
            .Match(filter)
            .ToList();
        return result;
    }

    public IList<TemperatureEntity<string>> GetByDate(long dateFrom, long dateTo)
    {
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .And(Builders<TemperatureEntity<string>>.Filter.Gte(m => m.Date, dateFrom), Builders<TemperatureEntity<string>>.Filter.Lte(m => m.Date, dateTo));
        var result = _plusTemperatureCollection.Aggregate()
            .Match(filter)
            .ToList();
        return result;
    }

    public async Task DeleteAll()
    {
        await _plusTemperatureCollection.DeleteManyAsync(Builders<TemperatureEntity<string>>.Filter.Empty);
    }
    public IEnumerable<TemperatureEntity<string>> GetAll()
    {
        var result = _plusTemperatureCollection.Aggregate().ToList();
        return result;
    }
}