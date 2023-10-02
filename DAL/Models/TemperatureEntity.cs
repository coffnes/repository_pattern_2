namespace RepoTask.DAL.Models;

public abstract class TemperatureEntity<T> : Entity<T>, IEquatable<TemperatureEntity<T>>
{
    public double Temperature { get; set; }
    public long Date { get; set; }
    public string? City { get; set; }
    public double Cloudiness { get; set; }
    public double Wetness { get; set; }
    public double WindSpeed { get; set; }
    public int Pressure { get; set; }
    public string? Summary { get; set; }
    public bool Equals(TemperatureEntity<T> other)
    {
        if (other is null)
            return false;

        return this.Temperature.Equals(other.Temperature) && this.Date == other.Date && this.City == other.City;
    }

    public override bool Equals(object obj) => Equals(obj as TemperatureEntity<T>);
    public override int GetHashCode() => Id.Timestamp.GetHashCode();
}