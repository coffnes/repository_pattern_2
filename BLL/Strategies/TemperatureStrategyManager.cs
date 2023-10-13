using RepoTask.DAL;

namespace RepoTask.BLL.Strategies;

public class TemperatureStrategyManager : IStrategyManager<Temperature>
{
    private readonly IEnumerable<IStrategy<Temperature>> _strategies;
    private readonly IDefaultStrategy<Temperature> _defaultStrategy;

    public TemperatureStrategyManager(IEnumerable<IStrategy<Temperature>> strategies, IDefaultStrategy<Temperature> defaultStrategy)
    {
        _strategies = strategies;
        _defaultStrategy = defaultStrategy;
    }

    public IStrategy<Temperature> GetStrategy(Temperature t)
    {
        foreach(var s in _strategies)
        {
            if(s.Algorithm(t))
            {
                return s;
            }
        }
        return _defaultStrategy;
    }
}