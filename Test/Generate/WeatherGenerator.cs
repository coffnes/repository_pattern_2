using RepoTask.DAL;
using RepoTask.DAL.Models;
using RepoTask.DAL.Repositories;
using RepoTask.BLL;
using System.Text.Json;
using System.Collections.Concurrent;
using Bogus;

namespace RepoTask.Test.Generate;

public class WeatherGenerator : IHostedService
{
    private readonly WeatherHandler _handler;
    private readonly Random rnd;
    private readonly MongoRepositoryManager _manager;
    
    public static readonly IList<City> cities = JsonSerializer.Deserialize<List<City>>(File.ReadAllText("Test/Generate/russian-cities.json"));

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

    public async void Generate()
    {
        // for(int i = 0; i < 40; i++)
        // {
        //     ThreadPool.QueueUserWorkItem((state) => GeneratePlusTemperatures());
        //     ThreadPool.QueueUserWorkItem((state) => GenerateMinusTemperatures());
        //     ThreadPool.QueueUserWorkItem((state) => GenerateZeroTemperatures());
        // }
        // Parallel.For(0, 10, (index, state) => {
        //     GeneratePlusTemperatures();
        //     GenerateMinusTemperatures();
        //     GenerateZeroTemperatures();
        // });
        // for(var i = 0; i < 40; i++)
        // {
        //     GeneratePlusTemperatures();
        //     GenerateMinusTemperatures();
        //     GenerateZeroTemperatures();
        // }
        var testPlusWeathers = new Faker<WeatherForecast>()
            .RuleFor(o => o.City, f => f.PickRandom(cities).name)
            .RuleFor(o => o.Cloudiness, f => f.Random.Double(0, 1))
            .RuleFor(o => o.Wetness, f => f.Random.Double(0, 1))
            .RuleFor(o => o.Date, f => f.Random.Long(1696107600, 1698613200))
            .RuleFor(o => o.Pressure, f => f.Random.Int(740, 770))
            .RuleFor(o => o.Temperature, f => f.Random.Int(1, 50))
            .RuleFor(o => o.Summary, f => f.Name.FirstName())
            .RuleFor(o => o.WindSpeed, f => f.Random.Double(0, 6));
        var testMinusWeathers = new Faker<WeatherForecast>()
            .RuleFor(o => o.City, f => f.PickRandom(cities).name)
            .RuleFor(o => o.Cloudiness, f => f.Random.Double(0, 1))
            .RuleFor(o => o.Wetness, f => f.Random.Double(0, 1))
            .RuleFor(o => o.Date, f => f.Random.Long(1696107600, 1698613200))
            .RuleFor(o => o.Pressure, f => f.Random.Int(740, 770))
            .RuleFor(o => o.Temperature, f => f.Random.Int(-50, -1))
            .RuleFor(o => o.Summary, f => f.Name.FirstName())
            .RuleFor(o => o.WindSpeed, f => f.Random.Double(0, 6));
        var testZeroesWeathers = new Faker<WeatherForecast>()
            .RuleFor(o => o.City, f => f.PickRandom(cities).name)
            .RuleFor(o => o.Cloudiness, f => f.Random.Double(0, 1))
            .RuleFor(o => o.Wetness, f => f.Random.Double(0, 1))
            .RuleFor(o => o.Date, f => f.Random.Long(1696107600, 1698613200))
            .RuleFor(o => o.Pressure, f => f.Random.Int(740, 770))
            .RuleFor(o => o.Temperature, f => f.Random.Int(0, 0))
            .RuleFor(o => o.Summary, f => f.Name.FirstName())
            .RuleFor(o => o.WindSpeed, f => f.Random.Double(0, 6));
        Temperature minus = new()
        {
            TemperatureC = -1,
        };
        Temperature plus = new()
        {
            TemperatureC = 1,
        };
        Temperature zero = new()
        {
            TemperatureC = 0,
        };
        // for(int i = 0; i < 10; i++)
        // {
        //     List<TemperatureEntity<string>> plusChunk = new List<TemperatureEntity<string>>(testPlusWeathers.GenerateBetween(10000, 10000).Cast<TemperatureEntity<string>>());
        //     List<TemperatureEntity<string>> minusChunk = new List<TemperatureEntity<string>>(testMinusWeathers.GenerateBetween(10000, 10000).Cast<TemperatureEntity<string>>());
        //     List<TemperatureEntity<string>> zeroesChunk = new List<TemperatureEntity<string>>(testZeroesWeathers.GenerateBetween(10000, 10000).Cast<TemperatureEntity<string>>());
        //     await _handler.HandlChunk(minusChunk, minus);
        //     await _handler.HandlChunk(plusChunk, plus);
        //     await _handler.HandlChunk(zeroesChunk, zero);
        // }
        Parallel.For(0, 10, async (index) => {
            List<TemperatureEntity<string>> plusChunk = new List<TemperatureEntity<string>>(testPlusWeathers.GenerateBetween(10000, 10000).Cast<TemperatureEntity<string>>());
            List<TemperatureEntity<string>> minusChunk = new List<TemperatureEntity<string>>(testMinusWeathers.GenerateBetween(10000, 10000).Cast<TemperatureEntity<string>>());
            List<TemperatureEntity<string>> zeroesChunk = new List<TemperatureEntity<string>>(testZeroesWeathers.GenerateBetween(10000, 10000).Cast<TemperatureEntity<string>>());
            await _handler.HandlChunk(minusChunk, minus);
            await _handler.HandlChunk(plusChunk, plus);
            await _handler.HandlChunk(zeroesChunk, zero);
        });
    }

    public async void GeneratePlusTemperatures()
    {
        // List<TemperatureEntity<string>> chunk = new();
        // for(int i = 0; i < 2500; i++)
        // {
        //     WeatherForecast weather = new()
        //     {
        //         Temperature = GenerateTemperature(),
        //         City = GenerateCity(),
        //         Date = GenerateDate(),
        //         Cloudiness = GenerateCloudiness(),
        //         Wetness = GenerateWetness(),
        //         WindSpeed = GenerateWindSpeed(),
        //         Pressure = GeneratePressure(),
        //         Summary = GenerateSummary()
        //     };
        //     chunk.Add(weather);
        // }
        // Temperature t = new()
        // {
        //     TemperatureC = 1,
        // };
        // await _handler.HandlChunk(chunk, t);
        Parallel.ForEach(Partitioner.Create(0, 250), async (range) => {
            List<TemperatureEntity<string>> chunk = new();
            for(int i = range.Item1; i < range.Item2; i++)
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
            await _handler.HandlChunk(chunk, t);
        });
    }
    public async void GenerateMinusTemperatures()
    {
        // List<TemperatureEntity<string>> chunk = new();
        // for(int i = 0; i < 2500; i++)
        // {
        //     WeatherForecast weather = new()
        //     {
        //         Temperature = (-1)*GenerateTemperature(),
        //         City = GenerateCity(),
        //         Date = GenerateDate(),
        //         Cloudiness = GenerateCloudiness(),
        //         Wetness = GenerateWetness(),
        //         WindSpeed = GenerateWindSpeed(),
        //         Pressure = GeneratePressure(),
        //         Summary = GenerateSummary()
        //     };
        //     chunk.Add(weather);
        // }
        // Temperature t = new()
        // {
        //     TemperatureC = -1,
        // };
        // await _handler.HandlChunk(chunk, t);
        Parallel.ForEach(Partitioner.Create(0, 250), async (range) => {
            List<TemperatureEntity<string>> chunk = new();
            for(int i = range.Item1; i < range.Item2; i++)
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
            await _handler.HandlChunk(chunk, t);
        });
    }
    public async void GenerateZeroTemperatures()
    {
        // List<TemperatureEntity<string>> chunk = new();
        // for(int i = 0; i < 2500; i++)
        // {
        //     WeatherForecast weather = new()
        //     {
        //         Temperature = 0,
        //         City = GenerateCity(),
        //         Date = GenerateDate(),
        //         Cloudiness = GenerateCloudiness(),
        //         Wetness = GenerateWetness(),
        //         WindSpeed = GenerateWindSpeed(),
        //         Pressure = GeneratePressure(),
        //         Summary = GenerateSummary()
        //     };
        //     chunk.Add(weather);
        // }
        // Temperature t = new()
        // {
        //     TemperatureC = 0,
        // };
        // await _handler.HandlChunk(chunk, t);
        Parallel.ForEach(Partitioner.Create(0, 250), async (range) => {
            List<TemperatureEntity<string>> chunk = new();
            for(int i = range.Item1; i < range.Item2; i++)
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
            await _handler.HandlChunk(chunk, t);
        });
    }

    private int GenerateTemperature()
    {
        return rnd.Next(1, 35);
    }

    private string GenerateCity()
    {
        return cities[rnd.Next(cities.Count)].name;
    }
    private long GenerateDate()
    {
        DateTime start = new(2023, 11, 1);
        DateTime stop = new(2023, 11, 30);
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