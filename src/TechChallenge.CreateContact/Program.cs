using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.CreateContact.Api.Extensions;
using TechChallenge.CreateContact.Api.Middlewares;
using TechChallenge.CreateContact.Application.Extensions;
using TechChallenge.CreateContact.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddConfiguration();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenApi at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Postgres")!)
    .AddRabbitMQ(sp =>
    {
        var rabbitMqService = sp.GetRequiredService<IRabbitMqService>();
        return rabbitMqService.Connection;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpMetrics();
app.MapMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandlingMiddleware();

app.MapControllers();

app.ApplyMigrations();
await app.WarmUpCache();

app.Run();

public partial class Program { }