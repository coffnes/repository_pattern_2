using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class WetnessFilterHandler : FilterHandler
{

    public WetnessFilterHandler(FilterHandler successor)
    {
        Successor = successor;
    }

    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        if(selectedSort == "wetness")
        {
            collection.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                return x.Wetness.CompareTo(y.Wetness);
            });
        }
        else
        {
            Successor.HandleCollection(selectedSort, collection);
        }
        return collection;
    }
}