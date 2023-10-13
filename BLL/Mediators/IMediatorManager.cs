using RepoTask.DAL.Repositories;

namespace RepoTask.BLL.Mediators;

public interface IMediatorManager<T, K>
{
    public IMongoRepository<T> GetCurrentRepository(K resolveParam);
}