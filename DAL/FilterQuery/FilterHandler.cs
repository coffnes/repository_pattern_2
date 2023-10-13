using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public abstract class FilterHandler
{
    public FilterHandler Successor{ get; set; }
    public abstract IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection);
}