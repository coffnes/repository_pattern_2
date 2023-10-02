using RepoTask.DAL.Repositories;
using RepoTask.BLL.Strategies;

namespace RepoTask.BLL.Mediators;

public interface IMediator<T, K>
{
    public IMongoRepository<T> GetRepository();
    public bool StrategyTypeCheck(IStrategy<K> strategy);
}