using RepoTask.DAL.FilterQuery;
using RepoTask.DAL.Models;

namespace RepoTask.DAL.Repositories;

public class MongoRepositoryManager
{
    private readonly IEnumerable<IMongoRepository<string>> _repositories;
    private readonly IDefaultRepository<string> _defaultRepository;
    private readonly TemperatureFilterHandler _tempHadler;

    public MongoRepositoryManager(IEnumerable<IMongoRepository<string>> repositories, IDefaultRepository<string> defaultRepository, TemperatureFilterHandler tempHadler)
    {
        _repositories = repositories;
        _defaultRepository = defaultRepository;
        _tempHadler = tempHadler;
    }

    public IEnumerable<TemperatureEntity<string>> GetAll()
    {
        List<TemperatureEntity<string>> result = new();
        foreach(var r in _repositories)
        {
            result.AddRange(r.GetAll().Result);
        }
        return result;
    }

    public IList<TemperatureEntity<string>> GetByCity(string? city)
    {
        List<TemperatureEntity<string>> result = new();
        foreach(var r in _repositories)
        {
            result = result.Union(r.GetByCity(city)).ToList();
        }
        return result;
    }

    public IList<TemperatureEntity<string>> GetByDate(long dateFrom, long dateTo)
    {
        List<TemperatureEntity<string>> result = new();
        foreach(var r in _repositories)
        {
            result = result.Union(r.GetByDate(dateFrom, dateTo)).ToList();
        }
        return result;
    }
    public async Task DeleteAll()
    {
        foreach(var r in _repositories)
        {
            await r.DeleteAll();
        }
    }
    public async Task<IList<TemperatureEntity<string>>> GetOnlyZeroTemperature()
    {
        var result = await _defaultRepository.GetAll();
        return result.ToList();
    }
    public IList<TemperatureEntity<string>> GetByFilter(FilterOptions filter)
    {
        List<TemperatureEntity<string>> result = new();
        List<TemperatureEntity<string>> resultByCity = new();
        List<TemperatureEntity<string>> resultByDate = new();
        foreach(var r in _repositories)
        {
            var filteredByCity = r.GetByCity(filter.selectedCity).ToList();
            resultByCity = resultByCity.Concat(filteredByCity).ToList();
            if(filter.selectedDateFrom != "" && filter.selectedDateTo != "") {
                var filteredByDate = r.GetByDate((long)Convert.ToDouble(filter.selectedDateFrom), (long)Convert.ToDouble(filter.selectedDateTo));
                resultByDate = resultByDate.Union(filteredByDate).ToList();
            }
        }
        if(resultByDate.Count != 0)
            result = resultByCity.Intersect(resultByDate).ToList();
        else
            result = resultByCity.ToList();
        var sortingResult = _tempHadler.HandleCollection(filter.selectedSort, result);

        return sortingResult;
    }

    public IList<TemperatureEntity<string>> Search(string searchQuery)
    {
        List<TemperatureEntity<string>> result = new();
        foreach(var r in _repositories)
        {
            result = result.Concat(r.Search(searchQuery)).ToList();
        }
        return result;
    }
}