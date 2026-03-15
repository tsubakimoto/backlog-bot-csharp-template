using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//builder.Services
//    .AddApplicationInsightsTelemetryWorkerService()
//    .ConfigureFunctionsApplicationInsights();

// Backlog API
builder.Services.AddHttpClient("BacklogApi", client =>
{
    var spaceId = builder.Configuration["Backlog:SpaceId"];
    client.BaseAddress = new Uri($"https://{spaceId}.backlog.com");
    client.DefaultRequestHeaders.Accept.Add(new("application/x-www-form-urlencoded"));
});

builder.Services.AddDistributedMemoryCache();

builder.Build().Run();
