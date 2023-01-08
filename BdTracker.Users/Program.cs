using System.Text.Json.Serialization;
using BdTracker.Back.Data;
using BdTracker.Back.Services.Interfaces;
using BdTracker.Shared.Entities;
using BdTracker.Users.Helpers;
using GameStore.Core.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration["PostgreSQL:ConnectionName"]);
dataSourceBuilder.MapEnum<Sex>();
await using var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(dataSource)/*.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)*/;
});

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));

// builder.Services.Configure<JsonOptions>(opt =>
// {
//     opt.SerializerOptions.Converters.Add(new DateOnlyConverter());
// });

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // serialize DateOnly as strings
        o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        o.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });

builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
