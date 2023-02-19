using BdTracker.Groups.Data;
using BdTracker.Groups.Dtos.Requests;
using BdTracker.Groups.Dtos.Responses;
using BdTracker.Shared.Infrastructure;
using BdTracker.Shared.Services;
using BdTracker.Shared.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BdTracker.Groups.Entities;

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/groups", async ([FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var groups = await context.Groups.ToListAsync();
    return mapper.Map<List<GroupResponse>>(groups);
})
.WithName("GetAllGroups")
.Produces<List<GroupResponse>>()
.WithOpenApi();

app.MapGet("/groups/{id:guid}", async (Guid id, [FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var group = await context.Groups.FindAsync(id);
    return group == null ? Results.NotFound() : Results.Ok(mapper.Map<GroupResponse>(group));
})
.WithName("GetSingleGroup")
.Produces<GroupResponse>()
.WithOpenApi();

app.MapPost("/groups", async (CreateGroupRequest request, [FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var group = mapper.Map<Group>(request);
    var result = await context.Groups.AddAsync(group);
    await context.SaveChangesAsync();
    return Results.Ok(mapper.Map<GroupResponse>(result.Entity));
})
.WithName("CreateGroup")
.Produces<GroupResponse>()
.WithOpenApi();

app.MapPut("/groups/{id:guid}", async (Guid id, UpdateGroupRequest request, [FromServices] AppDbContext context) =>
{
    var rowAffected = await context.Groups.Where(g => g.Id == id)
        .ExecuteUpdateAsync(update =>
        update.SetProperty(g => g.Name, request.Name));

    return rowAffected == 0 ? Results.NotFound() : Results.NoContent();
})
.WithName("UpdateGroup")
.Produces(401)
.Produces(201)
.WithOpenApi();

app.MapDelete("groups/{id:guid}", async (Guid id, [FromServices] AppDbContext context) =>
{
    var rowAffected = await context.Groups.Where(g => g.Id == id)
        .ExecuteDeleteAsync();

    return rowAffected == 0 ? Results.NotFound() : Results.NoContent();
})
.WithName("DeleteGroup")
.Produces(404)
.Produces(201)
.WithOpenApi();

app.Run();