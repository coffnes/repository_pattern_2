using RepoTask.DAL;
using RepoTask.DAL.Repositories;
using RepoTask.BLL.Strategies;

namespace RepoTask.BLL.Mediators;

public class DefaultMediator : IMediator<string, Temperature>
{
    private readonly IDefaultRepository<string> _repository;

    public DefaultMediator(IDefaultRepository<string> repository)
    {
        _repository = repository;
    }

    public IMongoRepository<string> GetRepository()
    {
        return _repository;
    }

    public bool StrategyTypeCheck(IStrategy<Temperature> s)
    {
        return s is IDefaultStrategy<Temperature>;
    }
}