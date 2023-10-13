using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class PressureFilterHandler : FilterHandler
{

    public PressureFilterHandler(FilterHandler successor)
    {
        Successor = successor;
    }

    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        if(selectedSort == "pressure")
        {
            collection.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                return x.Pressure.CompareTo(y.Pressure);
            });
        }
        else
        {
            Successor.HandleCollection(selectedSort, collection);
        }
        return collection;
    }
}