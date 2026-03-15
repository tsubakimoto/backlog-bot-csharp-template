using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BacklogBotApp;

public class Bot
{
    private static readonly string issueAddEndpoint = "/api/v2/issues";

    private readonly ILogger<Bot> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string apiKey;

    public Bot(
        ILogger<Bot> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;

        apiKey = configuration["Backlog:ApiKey"] ?? throw new ArgumentNullException("Backlog:ApiKey");
    }

    [Function("Bot")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        logger.LogDebug("Request body: {0}", requestBody);

        var json = JsonSerializer.Deserialize<BacklogBody>(requestBody);
        if (json is null)
        {
            logger.LogError("Failed to deserialize request body.");
            return new BadRequestResult();
        }

        logger.LogInformation($"Received event: {json.type} for project {json.project.name} by user {json.createdUser.name} at {json.created}");
        switch (json.type)
        {
            case BacklogBodyType.IssueAdded:
                return new OkResult();
            case BacklogBodyType.Commented:
                return new OkResult();
            default:
                return new BadRequestResult();
        }
    }
}
