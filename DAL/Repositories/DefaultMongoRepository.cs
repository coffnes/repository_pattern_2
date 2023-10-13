using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RepoTask.DAL.Models;
using MongoDB.Bson;

namespace RepoTask.DAL.Repositories;

public class DefaultMongoRepository : IDefaultRepository<string>
{
    private readonly IMongoCollection<TemperatureEntity<string>> _zeroTemperatureCollection;
    public DefaultMongoRepository(ZeroMongoClient mongoClient, IOptions<ZeroMongoDatabaseSettings> weatherDatabaseSettings)
    {
        var mongoDatabse = mongoClient.MongoClient.GetDatabase(weatherDatabaseSettings.Value.DatabaseName);
        _zeroTemperatureCollection = mongoDatabse.GetCollection<TemperatureEntity<string>>(weatherDatabaseSettings.Value.ZeroTemperatureCollectionName);
        var options = new CreateIndexOptions { Unique = false };
        _zeroTemperatureCollection.Indexes.CreateOne("{ City : \"text\" }", options);
    }
    public async Task AddAsync(TemperatureEntity<string> entity)
    {
        await _zeroTemperatureCollection.InsertOneAsync(entity);
    }

    public async Task AddChunkAsync(IList<TemperatureEntity<string>> entities)
    {
        await _zeroTemperatureCollection.InsertManyAsync(entities);
    }
    public IList<TemperatureEntity<string>> GetByCity(string? city)
    {
        if(city == "" || city == "None") {
            return _zeroTemperatureCollection.Aggregate().ToList();
        }
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .Eq(m => m.City, city);
        var result = _zeroTemperatureCollection.Aggregate()
            .Match(filter)
            .ToList();
        return result;
    }
    public IList<TemperatureEntity<string>> GetByDate(long dateFrom, long dateTo)
    {
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .And(Builders<TemperatureEntity<string>>.Filter.Gt(m => m.Date, dateFrom), Builders<TemperatureEntity<string>>.Filter.Lt(m => m.Date, dateTo));
        var result = _zeroTemperatureCollection.Aggregate()
            .Match(filter)
            .ToList();
        return result;
    }
    public async Task DeleteAll()
    {
        await _zeroTemperatureCollection.DeleteManyAsync(Builders<TemperatureEntity<string>>.Filter.Empty);
    }
    public async Task<IEnumerable<TemperatureEntity<string>>> GetAll()
    {
        var pipeline = new EmptyPipelineDefinition<TemperatureEntity<string>>();
        var cursor = await _zeroTemperatureCollection.AggregateAsync(pipeline: pipeline);
        var result = await cursor.ToListAsync();
        return result;
    }
    public IList<TemperatureEntity<string>> Search(string searchQuery)
    {
        var queryRegexp = new BsonRegularExpression($"{searchQuery}");
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .Regex("City", queryRegexp);
        //var result = _minusTemperatureCollection.Aggregate().Search(Builders<TemperatureEntity<string>>.Search.Autocomplete(s => s.City, searchQuery)).ToList();
        var result = _zeroTemperatureCollection.Aggregate().Match(filter).ToList();
        return result;
    }
}