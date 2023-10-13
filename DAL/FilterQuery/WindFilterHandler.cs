using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class WindFilterHandler : FilterHandler
{

    public WindFilterHandler(FilterHandler successor)
    {
        Successor = successor;
    }

    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        if(selectedSort == "windSpeed")
        {
            collection.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                return x.WindSpeed.CompareTo(y.WindSpeed);
            });
        }
        else
        {
            Successor.HandleCollection(selectedSort, collection);
        }
        return collection;
    }
}