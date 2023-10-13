namespace RepoTask.BLL.Strategies;

public interface IStrategy<T>
{
    public bool Algorithm(T resolveParam);
}