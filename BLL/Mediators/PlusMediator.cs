using RepoTask.DAL;
using RepoTask.DAL.Repositories;
using RepoTask.BLL.Strategies;

namespace RepoTask.BLL.Mediators;

public class PlusMediator : IMediator<string, Temperature>
{
    private readonly IPlusRepository<string> _repository;

    public PlusMediator(IPlusRepository<string> repository)
    {
        _repository = repository;
    }

    public IMongoRepository<string> GetRepository()
    {
        return _repository;
    }

    public bool StrategyTypeCheck(IStrategy<Temperature> s)
    {
        return s is IPlusStrategy<Temperature>;
    }
}