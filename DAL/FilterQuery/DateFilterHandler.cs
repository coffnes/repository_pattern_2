using RepoTask.DAL.Models;

namespace RepoTask.DAL.FilterQuery;

public class DateFilterHandler : FilterHandler
{
    public DateFilterHandler(FilterHandler successor)
    {
        Successor = successor;
    }

    public override IList<TemperatureEntity<string>> HandleCollection(string selectedSort, List<TemperatureEntity<string>> collection)
    {
        if(selectedSort == "date")
        {
            collection.Sort((TemperatureEntity<string> x, TemperatureEntity<string> y) =>
            {
                return x.Date.CompareTo(y.Date);
            });
        }
        else
        {
            Successor.HandleCollection(selectedSort, collection);
        }
        return collection;
    }
}