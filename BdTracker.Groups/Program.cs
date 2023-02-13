using BdTracker.Groups.Data;
using BdTracker.Groups.Dtos.Responses;
using BdTracker.Groups.Infrastructure;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Services;
using BdTracker.Shared.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration["PostgreSQL:ConnectionName"]);
await using var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(dataSource)/*.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)*/;
});

builder.Services.AddMapster();

builder.Services.AddScoped(typeof(DbContext), typeof(AppDbContext));
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(option => option.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", async ([FromServices] IService<Group> groupsRepository, [FromServices] IMapper mapper) =>
{
    var groups = await groupsRepository.GetAllAsync();
    return mapper.Map<List<GroupDto>>(groups);
})
    .WithName("GetAllGroups")
    .WithOpenApi();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
