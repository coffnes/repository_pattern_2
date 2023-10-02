using RepoTask.DAL;

namespace RepoTask.BLL.Strategies;

public class DefaultTemperatureStrategy : IDefaultStrategy<Temperature>
{
    public bool Algorithm(Temperature t)
    {
        return false;
    }
}