using RepoTask.DAL;
using RepoTask.DAL.Repositories;
using RepoTask.BLL.Strategies;

namespace RepoTask.BLL.Mediators;

public class TemperatureMediatorManager : IMediatorManager<string, Temperature>
{
    private readonly IEnumerable<IMediator<string, Temperature>> _mediators;
    private readonly IStrategyManager<Temperature> _strategyManager;
    private readonly IDefaultRepository<string> _defaultRepository;

    public TemperatureMediatorManager(IEnumerable<IMediator<string, Temperature>> mediators, IStrategyManager<Temperature> strategyManager, IDefaultRepository<string> defaultRepository)
    {
        _mediators = mediators;
        _strategyManager = strategyManager;
        _defaultRepository = defaultRepository;
    }

    public IMongoRepository<string> GetCurrentRepository(Temperature t)
    {
        foreach(var m in _mediators)
        {
            if(m.StrategyTypeCheck(_strategyManager.GetStrategy(t)))
            {
                return m.GetRepository();
            }
        }
        return _defaultRepository;
    }
}