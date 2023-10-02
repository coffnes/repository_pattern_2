using RepoTask.DAL;
using RepoTask.DAL.Models;
using RepoTask.DAL.Repositories;
using RepoTask.BLL;
using System.Text.Json;

namespace RepoTask.Test.Generate;

public class WeatherGenerator : IHostedService
{
    private readonly WeatherHandler _handler;
    private readonly Random rnd;
    private readonly MongoRepositoryManager _manager;

    public WeatherGenerator(WeatherHandler handler, MongoRepositoryManager manager)
    {
        _manager = manager;
        _handler = handler;
        rnd = new();
    }

    public async Task Delete()
    {
        await _manager.DeleteAll();
    }

    public void Generate()
    {
        for(int i = 0; i < 4; i++)
        {
            ThreadPool.QueueUserWorkItem((state) => GeneratePlusTemperatures());
            ThreadPool.QueueUserWorkItem((state) => GenerateMinusTemperatures());
            ThreadPool.QueueUserWorkItem((state) => GenerateZeroTemperatures());
        }
    }

    public void GeneratePlusTemperatures()
    {
        List<TemperatureEntity<string>> chunk = new();
        for(int i = 0; i < 250; i++)
        {
            WeatherForecast weather = new()
            {
                Temperature = GenerateTemperature(),
                City = GenerateCity(),
                Date = GenerateDate(),
                Cloudiness = GenerateCloudiness(),
                Wetness = GenerateWetness(),
                WindSpeed = GenerateWindSpeed(),
                Pressure = GeneratePressure(),
                Summary = GenerateSummary()
            };
            chunk.Add(weather);
        }
        Temperature t = new()
        {
            TemperatureC = 1,
        };
        _handler.HandlChunk(chunk, t);
    }
    public void GenerateMinusTemperatures()
    {
        List<TemperatureEntity<string>> chunk = new();
        for(int i = 0; i < 250; i++)
        {
            WeatherForecast weather = new()
            {
                Temperature = (-1)*GenerateTemperature(),
                City = GenerateCity(),
                Date = GenerateDate(),
                Cloudiness = GenerateCloudiness(),
                Wetness = GenerateWetness(),
                WindSpeed = GenerateWindSpeed(),
                Pressure = GeneratePressure(),
                Summary = GenerateSummary()
            };
            chunk.Add(weather);
        }
        Temperature t = new()
        {
            TemperatureC = -1,
        };
        _handler.HandlChunk(chunk, t);
    }
    public void GenerateZeroTemperatures()
    {
        List<TemperatureEntity<string>> chunk = new();
        for(int i = 0; i < 250; i++)
        {
            WeatherForecast weather = new()
            {
                Temperature = 0,
                City = GenerateCity(),
                Date = GenerateDate(),
                Cloudiness = GenerateCloudiness(),
                Wetness = GenerateWetness(),
                WindSpeed = GenerateWindSpeed(),
                Pressure = GeneratePressure(),
                Summary = GenerateSummary()
            };
            chunk.Add(weather);
        }
        Temperature t = new()
        {
            TemperatureC = 0,
        };
        _handler.HandlChunk(chunk, t);
    }

    private int GenerateTemperature()
    {
        return rnd.Next(1, 35);
    }

    private string GenerateCity()
    {
        var cities = JsonSerializer.Deserialize<List<City>>(File.ReadAllText("Test/Generate/russian-cities.json"));
        return cities[rnd.Next(cities.Count)].name;
    }

    private long GenerateDate()
    {
        DateTime start = new(2023, 9, 1);
        DateTime stop = new(2023, 9, 30);
        int range = (stop - start).Days;
        return (long)start.AddDays(rnd.Next(range)).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
    
    private double GenerateCloudiness()
    {
        return rnd.NextDouble();
    }

    private double GenerateWetness()
    {
        return rnd.NextDouble();
    }

    private double GenerateWindSpeed()
    {
        return rnd.NextDouble() * 10;
    }

    private int GeneratePressure()
    {
        return rnd.Next(740, 770);
    }
    
    private string GenerateSummary()
    {
        List<string> summaries = new(){ "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        return summaries[rnd.Next(summaries.Count)];
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Delete();
        Generate();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Delete();
    }
}