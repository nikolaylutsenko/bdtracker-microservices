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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"]!))
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin", "SuperAdmin"))
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin", "SuperAdmin"))
    .AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthorization();

app.UseHttpsRedirection();

var groups = app.MapGroup("/api/v1/groups")
    .WithOpenApi()
    .RequireAuthorization("UserPolicy");

groups.MapGet("/", async ([FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var groups = await context.Groups.ToListAsync();
    return mapper.Map<List<GroupResponse>>(groups);
})
.WithName("GetAllGroups")
.Produces<List<GroupResponse>>()
.WithTags("Groups");

groups.MapGet("/{id:guid}", async (Guid id, [FromServices] AppDbContext context, [FromServices] IMapper mapper) =>
{
    var group = await context.Groups.FindAsync(id);
    return group == null ? Results.NotFound() : Results.Ok(mapper.Map<GroupResponse>(group));
})
.WithName("GetSingleGroup")
.Produces<GroupResponse>()
.WithTags("Groups");

groups.MapPost("/", async (CreateGroupRequest request, [FromServices] AppDbContext context, [FromServices] IMapper mapper, HttpRequest httpRequest) =>
{
    var group = mapper.Map<Group>(request);
    group.CreatedDate = DateTime.UtcNow;
    group.CreatedBy = httpRequest.HttpContext.User.FindFirst(c => c.Type == "Id")?.Value ?? Guid.Empty.ToString();
    var result = await context.Groups.AddAsync(group);
    await context.SaveChangesAsync();
    return Results.Ok(mapper.Map<GroupResponse>(result.Entity));
})
.WithName("CreateGroup")
.Accepts<CreateGroupRequest>("application/json")
.Produces<GroupResponse>()
.WithTags("Groups");

groups.MapPut("/{id:guid}", async (Guid id, UpdateGroupRequest request, [FromServices] AppDbContext context, HttpRequest httpRequest) =>
{
    var userId = httpRequest.HttpContext.User.FindFirst(c => c.Type == "Id")?.Value ?? Guid.Empty.ToString();
    var rowAffected = await context.Groups.Where(g => g.Id == id)
        .ExecuteUpdateAsync(update =>
            update.SetProperty(g => g.Name, request.Name)
                  .SetProperty(g => g.EditedBy, userId));

    return rowAffected == 0 ? Results.NotFound() : Results.NoContent();
})
.WithName("UpdateGroup")
.Produces(401)
.Produces(201)
.WithTags("Groups");

groups.MapDelete("/{id:guid}", async (Guid id, [FromServices] AppDbContext context) =>
{
    var rowAffected = await context.Groups.Where(g => g.Id == id)
        .ExecuteDeleteAsync();

    return rowAffected == 0 ? Results.NotFound() : Results.NoContent();
})
.WithName("DeleteGroup")
.Produces(404)
.Produces(201)
.WithTags("Groups");

app.Run();