using RepoTask.DAL;

namespace RepoTask.BLL.Strategies;

public class MinusTemperatureStrategy : IMinusStrategy<Temperature>
{
    public bool Algorithm(Temperature t)
    {
        return t.TemperatureC < 0;
    }
}