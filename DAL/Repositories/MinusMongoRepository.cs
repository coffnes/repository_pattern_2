using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RepoTask.DAL.Models;

namespace RepoTask.DAL.Repositories;

public class MinusMongoRepository : IMinusRepository<string>
{
    private readonly IMongoCollection<TemperatureEntity<string>> _minusTemperatureCollection;
    private readonly IMongoCollection<TemperatureEntity<string>> _minusTemperatureCollectionOut;
    public MinusMongoRepository(MinusMongoClient mongoClient, IOptions<MinusMongoDatabaseSettings> databaseSettings)
    {
        var mongoDatabase = mongoClient.MongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _minusTemperatureCollection = mongoDatabase.GetCollection<TemperatureEntity<string>>(databaseSettings.Value.MinusTemperatureCollectionName);
        _minusTemperatureCollectionOut = mongoDatabase.GetCollection<TemperatureEntity<string>>(databaseSettings.Value.MinusTemperatureCollectionName);
        var options = new CreateIndexOptions { Unique = false };
        _minusTemperatureCollection.Indexes.CreateOne("{ City : \"text\" }", options);
    }
    public async Task AddAsync(TemperatureEntity<string> entity)
    {
        await _minusTemperatureCollection.InsertOneAsync(entity);
    }
    public async Task AddChunkAsync(IList<TemperatureEntity<string>> entities)
    {
        await _minusTemperatureCollection.InsertManyAsync(entities);
    }
    public IList<TemperatureEntity<string>> GetByCity(string? city)
    {
        List<TemperatureEntity<string>> result = new();
        if(city == "" || city == "None") {
            return _minusTemperatureCollection.Aggregate().ToList();
        }
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .Eq(m => m.City, city);
        return _minusTemperatureCollection.Aggregate()
            .Match(filter)
            .ToList();
    }
    public IList<TemperatureEntity<string>> GetByDate(long dateFrom, long dateTo)
    {
        var filter = Builders<TemperatureEntity<string>>
            .Filter
            .And(Builders<TemperatureEntity<string>>.Filter.Gt(m => m.Date, dateFrom), Builders<TemperatureEntity<string>>.Filter.Lt(m => m.Date, dateTo));
        var result = _minusTemperatureCollection.Aggregate()
            .Match(filter)
            .ToList();
        return result;
    }
    public async Task DeleteAll()
    {
        await _minusTemperatureCollection.DeleteManyAsync(Builders<TemperatureEntity<string>>.Filter.Empty);
    }
    public async Task<IEnumerable<TemperatureEntity<string>>> GetAll()
    {
        var pipeline = new EmptyPipelineDefinition<TemperatureEntity<string>>();
        var cursor = await _minusTemperatureCollectionOut.AggregateAsync(pipeline: pipeline);
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
        var result = _minusTemperatureCollection.Aggregate().Match(filter).ToList();
        return result;
    }
}