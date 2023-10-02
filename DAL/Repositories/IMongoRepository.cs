using RepoTask.DAL.Models;

namespace RepoTask.DAL.Repositories;

public interface IMongoRepository<T> : IRepository<T>
{
    public Task AddAsync(TemperatureEntity<T> entity);

    public Task AddChunkAsync(IList<TemperatureEntity<T>> entities);

    public IList<TemperatureEntity<T>> GetByCity(string? city);
    public IList<TemperatureEntity<T>> GetByDate(long dateFrom, long dateTo);
    public Task DeleteAll();
    public IEnumerable<TemperatureEntity<T>> GetAll();
}