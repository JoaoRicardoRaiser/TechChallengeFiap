using TechChallenge.Api.Extensions;
using TechChallenge.Infrastructure.Extensions;
using TechChallenge.Application.Extensions;
using TechChallenge.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseExceptionHandlingMiddleware();

app.ApplyMigrations();
await app.WarmUpCache();

app.Run();
