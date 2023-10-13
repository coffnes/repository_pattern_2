using MongoDB.Bson.IO;
using RepoTask.DAL;
using RepoTask.DAL.Models;

public class TemperatureEntityComparer<T> : IEqualityComparer<TemperatureEntity<T>>
{
    public bool Equals(TemperatureEntity<T> x, TemperatureEntity<T> y)
    {
        //Check whether the compared objects reference the same data.
        if (Object.ReferenceEquals(x, y)) return true;

        //Check whether any of the compared objects is null.
        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;

        //Check whether the products' properties are equal.
        return x.Temperature.Equals(y.Temperature) && x.City == y.City && x.Date == y.Date;
    }

    public int GetHashCode(TemperatureEntity<T> entity)
    {
        //Check whether the object is null
        if (Object.ReferenceEquals(entity, null)) return 0;

        //Calculate the hash code for the product.
        return entity.Id.Timestamp.GetHashCode();
    }
}