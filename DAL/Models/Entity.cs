using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RepoTask.DAL.Models;

public abstract class Entity<T>
{
    [BsonId]
    public ObjectId Id { get; set; }
}
