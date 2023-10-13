namespace RepoTask.BLL.Strategies;

public interface IStrategyManager<K>
{
    public IStrategy<K> GetStrategy(K resolveParam);
}