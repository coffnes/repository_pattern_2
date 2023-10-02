using RepoTask.DAL;
using RepoTask.DAL.Models;
using RepoTask.BLL.Mediators;

namespace RepoTask.BLL;

public class WeatherHandler
{
    private readonly IMediatorManager<string, Temperature> _manager;

    public WeatherHandler(IMediatorManager<string, Temperature> manager)
    {
        _manager = manager;
    }

    public Task Handl(WeatherForecast weatherForecast)
    {
        Temperature t = new()
        {
            TemperatureC = weatherForecast.Temperature
        };
        var repo = _manager.GetCurrentRepository(t);
        return repo.AddAsync(weatherForecast);
    }

    public void HandlChunk(IList<TemperatureEntity<string>> weathers, Temperature t)
    {
        var repo = _manager.GetCurrentRepository(t);
        repo.AddChunkAsync(weathers);
    }
}