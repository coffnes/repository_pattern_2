using RepoTask.DAL.Models;
using RepoTask.DAL.Repositories;

namespace RepoTask.DAL.GraphQL;

public class Query
{
    private readonly MongoRepositoryManager _repoManager;
    public Query(MongoRepositoryManager repoManager)
    {
        _repoManager = repoManager;
    }
    [UsePaging]
    public IEnumerable<TemperatureEntity<string>> GetWeathers(string selectedSort, string selectedCity, string selectedDateFrom, string selectedDateTo)
    {
        FilterOptions filter = new();
        filter.selectedSort = selectedSort;
        filter.selectedCity = selectedCity;
        filter.selectedDateFrom = selectedDateFrom;
        filter.selectedDateTo = selectedDateTo;
        var result = _repoManager.GetByFilter(filter);
        return result;
    }
}