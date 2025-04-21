using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TechChallenge.DeleteContact.Api.Extensions;
using TechChallenge.DeleteContact.Application.Extensions;
using TechChallenge.DeleteContact.Infrastructure.Extensions;
using TechChallenge.DeleteContact.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddConfiguration();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Postgres")!)
    .AddRabbitMQ(sp =>
    {
        var rabbitMqService = sp.GetRequiredService<IRabbitMqService>();
        return rabbitMqService.CreateConnectionAsync();
    });

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ApplyMigrations();

app.Run();

public partial class Program { }