using BdTracker.Back.Data;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BdTracker.Users.Dtos.Responses;
using MapsterMapper;
using BdTracker.Users.Entities;
using BdTracker.Users.Dtos.Requests;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(option => option.LowercaseUrls = true);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration["PostgreSQL:ConnectionName"]);
dataSourceBuilder.MapEnum<Sex>();
await using var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(dataSource)/*.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)*/;
});

builder.Services.AddMapster();

builder.Services.AddScoped(typeof(DbContext), typeof(AppDbContext));

builder.Services.Configure<JsonSerializerOptions>(options => options.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/v1/users", async ([FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var users = await context.Users.ToListAsync();
    return Results.Ok(mapper.Map<List<UserResponse>>(users));
})
.WithName("GetAllUsers")
.Produces<List<UserResponse>>()
.WithOpenApi();

app.MapGet("api/v1/users/{id:guid}", async (Guid id, [FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var user = await context.Users.FindAsync(id);
    return user == null ? Results.NotFound() : Results.Ok(mapper.Map<UserResponse>(user));
})
.WithName("GetUserById")
.Produces<UserResponse>()
.Produces(404)
.WithOpenApi();

app.MapPost("api/v1/users", async (CreateUserRequest request, [FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var user = mapper.Map<User>(request);
    var result = await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
    return Results.Ok(mapper.Map<UserResponse>(result.Entity));
})
.WithName("CreateUser")
.Produces<UserResponse>()
.WithOpenApi();

app.MapPut("api/v1/users/{id:guid}", async (Guid id, UpdateUserRequest request, [FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var rowAffected = await context.Users.Where(u => u.Id == id)
        .ExecuteUpdateAsync(updates =>
            updates.SetProperty(u => u.Name, request.Name)
                    .SetProperty(u => u.Surname, request.Surname)
                    .SetProperty(u => u.Sex, request.Sex)
                    .SetProperty(u => u.Birthday, request.Birthday)
                    .SetProperty(u => u.Occupation, request.Occupation)
                    .SetProperty(u => u.AboutMe, request.AboutMe)
                    .SetProperty(u => u.GroupsIds, request.GroupsIds));

    return rowAffected == 0 ? Results.NotFound() : Results.NoContent();
})
.WithName("UpdateUser")
.Produces(404)
.Produces(201)
.WithOpenApi();

app.MapDelete("api/v1/users/{id:guid}", async (Guid id, [FromServices] AppDbContext context) =>
{
    var rowAffected = await context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
    return rowAffected == 0 ? Results.NotFound() : Results.NoContent();
})
.WithName("DeleteUser")
.Produces(404)
.Produces(201)
.WithOpenApi();

app.Run();
