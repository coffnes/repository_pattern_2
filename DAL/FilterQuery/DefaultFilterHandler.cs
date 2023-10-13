using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class DefaultFilterHandler : FilterHandler
{
    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        return collection;
    }
}