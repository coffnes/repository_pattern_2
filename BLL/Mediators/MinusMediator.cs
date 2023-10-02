using RepoTask.DAL;
using RepoTask.DAL.Repositories;
using RepoTask.BLL.Strategies;

namespace RepoTask.BLL.Mediators;

public class MinusMediator : IMediator<string, Temperature>
{
    private readonly IMinusRepository<string> _repository;

    public MinusMediator(IMinusRepository<string> repository)
    {
        _repository = repository;
    }

    public IMongoRepository<string> GetRepository()
    {
        return _repository;
    }

    public bool StrategyTypeCheck(IStrategy<Temperature> s)
    {
        return s is IMinusStrategy<Temperature>;
    }
}