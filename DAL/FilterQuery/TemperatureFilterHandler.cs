using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class TemperatureFilterHandler : FilterHandler
{
    public TemperatureFilterHandler(FilterHandler successor)
    {
        Successor = successor;
    }

    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        if(selectedSort == "temperature")
        {
            collection.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                return x.Temperature.CompareTo(y.Temperature);
            });
        }
        else
        {
            Successor.HandleCollection(selectedSort, collection);
        }
        return collection;
    }
}