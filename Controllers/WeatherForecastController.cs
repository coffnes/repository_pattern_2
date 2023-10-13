using System.Text;
using Microsoft.AspNetCore.Mvc;
using RepoTask.DAL.Models;
using RepoTask.BLL;
using RepoTask.DAL.Repositories;
using RepoTask.DAL;
using System.Text.Json;
using RepoTask.Test.Generate;
using Microsoft.AspNetCore.ResponseCompression;

namespace RepoTask.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherHandler _handler;
    private readonly MongoRepositoryManager _repoManager;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherHandler handler,
        MongoRepositoryManager repoManager)
    {
        _logger = logger;
        _handler = handler;
        _repoManager = repoManager;
    }

    [HttpGet]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public IEnumerable<TemperatureEntity<string>> Get()
    {
        var result = _repoManager.GetAll();
        return result;
    }

    [HttpGet("get_cities")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 600)]
    public IList<City> GetCitiesList()
    {
        return WeatherGenerator.cities;
    }
    
    [HttpGet("city/{city?}")]
    public IList<TemperatureEntity<string>> GetByCity(string? city)
    {
        return _repoManager.GetByCity(city);
    }

    [HttpGet("date/{dateFrom}/{dateTo}")]
    public IList<TemperatureEntity<string>> GetByDate(long dateFrom, long dateTo)
    {
        return _repoManager.GetByDate(dateFrom, dateTo);
    }

    [HttpGet("zero")]
    public IList<TemperatureEntity<string>> GetOnlyZeroTemperature()
    {
        return _repoManager.GetOnlyZeroTemperature().Result;
    }

    [HttpPost]
    public async void Post(WeatherForecast weatherForecast)
    {
        await _handler.Handl(weatherForecast);
    }

    [HttpPost("query_post")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public async Task<string> FilterPost() {
        var jsonString = "";
        using(var inputStream = new StreamReader(HttpContext.Request.Body))
        {
            jsonString = await inputStream.ReadToEndAsync();
        }
        FilterOptions? filter = JsonSerializer.Deserialize<FilterOptions>(jsonString);
        var result = _repoManager.GetByFilter(filter);
        /*foreach (var r in result)
        {
            Console.WriteLine(r.City);
        }*/
        Console.WriteLine(result);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var jsonResponseString = "";
        //var jsonResponseString = JsonSerializer.Serialize(result, options);
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, result, options);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            jsonResponseString = await reader.ReadToEndAsync();
        }
        //await HttpContext.Response.Body.WriteAsync(new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(jsonResponseString))).AsTask();
        return jsonResponseString;
    }
    [HttpGet("filter_get")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 60)]
    public async Task<string> FilterGet() {
        FilterOptions filter = new FilterOptions();
        filter.selectedSort = Request.Query["selectedSort"];
        filter.selectedCity = Request.Query["selectedCity"];
        filter.selectedDateFrom = Request.Query["selectedDateFrom"];
        filter.selectedDateTo = Request.Query["selectedDateTo"];
        var result = _repoManager.GetByFilter(filter);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var jsonResponseString = "";
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, result, options);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            jsonResponseString = await reader.ReadToEndAsync();
        }
        return jsonResponseString;
    }

    [HttpGet("search/{searchQuery}")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public IList<TemperatureEntity<string>> Search(string searchQuery)
    {
        return _repoManager.Search(searchQuery);
    }
}
