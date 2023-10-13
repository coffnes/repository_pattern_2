using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class CloudinessFilterHandler : FilterHandler
{

    public CloudinessFilterHandler(FilterHandler successor)
    {
        Successor = successor;
    }

    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        if(selectedSort == "cloudiness")
        {
            collection.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                return x.Cloudiness.CompareTo(y.Cloudiness);
            });
        }
        else
        {
            Successor.HandleCollection(selectedSort, collection);
        }
        return collection;
    }
}