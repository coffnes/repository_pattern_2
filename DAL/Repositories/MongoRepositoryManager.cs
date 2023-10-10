using RepoTask.DAL.Models;

namespace RepoTask.DAL.Repositories;

public class MongoRepositoryManager
{
    private readonly IEnumerable<IMongoRepository<string>> _repositories;
    private readonly IDefaultRepository<string> _defaultRepository;

    public MongoRepositoryManager(IEnumerable<IMongoRepository<string>> repositories, IDefaultRepository<string> defaultRepository)
    {
        _repositories = repositories;
        _defaultRepository = defaultRepository;
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
        if(filter.selectedSort != "" && filter.selectedSort != "None")
        {
            result.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                if(filter.selectedSort == "date")
                {
                    return x.Date.CompareTo(y.Date);
                }
                if(filter.selectedSort == "city")
                {
                    return x.City.CompareTo(y.City);
                }
                if(filter.selectedSort == "temperatureC")
                {
                    return x.Temperature.CompareTo(y.Temperature);
                }
                return x.Date.CompareTo(y.Date);
            });
        }
        
        return result;
    }
}