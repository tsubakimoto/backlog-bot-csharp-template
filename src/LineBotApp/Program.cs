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

// LINE Messaging API
builder.Services.AddHttpClient("LineMessagingApi", client =>
{
    client.BaseAddress = new Uri("https://api.line.me/");
    client.DefaultRequestHeaders.Accept.Add(new("application/json"));
});
builder.Services.AddHttpClient("LineContentApi", client =>
{
    client.BaseAddress = new Uri("https://api-data.line.me/");
    client.DefaultRequestHeaders.Accept.Add(new("application/json"));
});

builder.Services.AddDistributedMemoryCache();

builder.Build().Run();
