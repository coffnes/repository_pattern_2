using RepoTask.DAL;
using RepoTask.DAL.Repositories;
using RepoTask.BLL.Strategies;
using RepoTask.BLL.Mediators;
using Microsoft.Extensions.Options;
using RepoTask.BLL;
using RepoTask.Test.Generate;
using VueCliMiddleware;
using Microsoft.AspNetCore.SpaServices;

var builder = WebApplication.CreateBuilder(args);

//Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "_myAllowSpecificOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    );
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add DB
builder.Services.Configure<PlusMongoDatabaseSettings>(builder.Configuration.GetSection("PlusMongoDatabase"));
builder.Services.Configure<MinusMongoDatabaseSettings>(builder.Configuration.GetSection("MinusMongoDatabase"));
builder.Services.Configure<ZeroMongoDatabaseSettings>(builder.Configuration.GetSection("ZeroMongoDatabase"));
builder.Services.AddSingleton((provider) => new MinusMongoClient(provider.GetRequiredService<IOptions<MinusMongoDatabaseSettings>>()));
builder.Services.AddSingleton((provider) => new PlusMongoClient(provider.GetRequiredService<IOptions<PlusMongoDatabaseSettings>>()));
builder.Services.AddSingleton((provider) => new ZeroMongoClient(provider.GetRequiredService<IOptions<ZeroMongoDatabaseSettings>>()));

//Add repositories
builder.Services.AddTransient<IMongoRepository<string>, MinusMongoRepository>();
builder.Services.AddTransient<IMongoRepository<string>, PlusMongoRepository>();
builder.Services.AddTransient<IMongoRepository<string>, DefaultMongoRepository>();
builder.Services.AddTransient<IMinusRepository<string>, MinusMongoRepository>();
builder.Services.AddTransient<IPlusRepository<string>, PlusMongoRepository>();
builder.Services.AddTransient<IDefaultRepository<string>, DefaultMongoRepository>();
builder.Services.AddTransient<MongoRepositoryManager>();

//Add strategies
builder.Services.AddTransient<IStrategy<Temperature>, MinusTemperatureStrategy>();
builder.Services.AddTransient<IStrategy<Temperature>, PlusTemperatureStrategy>();
builder.Services.AddTransient<IDefaultStrategy<Temperature>, DefaultTemperatureStrategy>();
builder.Services.AddTransient<IStrategyManager<Temperature>, TemperatureStrategyManager>();

//Add mediators
builder.Services.AddTransient<IMediator<string, Temperature>, MinusMediator>();
builder.Services.AddTransient<IMediator<string, Temperature>, PlusMediator>();
builder.Services.AddTransient<IMediator<string, Temperature>, DefaultMediator>();
builder.Services.AddTransient<IMediatorManager<string, Temperature>, TemperatureMediatorManager>();

//Weather heandler
builder.Services.AddTransient<WeatherHandler>();

//Generate development data
builder.Services.AddHostedService((provider) => new WeatherGenerator(provider.GetRequiredService<WeatherHandler>(), provider.GetRequiredService<MongoRepositoryManager>()));

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("_myAllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy();

if (app.Environment.IsDevelopment())
{
    app.MapToVueCliProxy(
        "{*path}",
        new SpaOptions { SourcePath = "ClientApp" },
        npmScript: "dev",
        port: 3399,
        regex: "Compiled successfully!",
        forceKill: true);
}

app.Run();
