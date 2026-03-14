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

namespace LineBotApp;

public class Bot
{
    private static readonly string _messagingApiEndpoint = "/v2/bot/message/reply";
    private static readonly string _contentApiEndpoint = "/v2/bot/message/{0}/content";

    private readonly ILogger<Bot> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string channelAccessToken;
    private readonly string channelSecret;

    public Bot(
        ILogger<Bot> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;

        channelAccessToken = configuration["Line:AccessToken"] ?? throw new ArgumentNullException("Line:AccessToken");
        channelSecret = configuration["Line:ChannelSecret"] ?? throw new ArgumentNullException("Line:ChannelSecret");
    }

    [Function("Bot")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        req.Headers.TryGetValue("X-Line-Signature", out var xLineSignature);
        if (string.IsNullOrEmpty(xLineSignature))
        {
            logger.LogError("Failed to get X-Line-Signature.");
            return new BadRequestResult();
        }

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        logger.LogDebug("Request body: {0}", requestBody);

        var json = JsonSerializer.Deserialize<LineMessageReceiveJson>(requestBody);
        if (json is null)
        {
            logger.LogError("Failed to deserialize request body.");
            return new BadRequestResult();
        }

        logger.LogDebug("Message: {0}", json.MessageText);

        if (IsSignature(xLineSignature!, requestBody, channelSecret!) && json.IsMessageEvent)
        {
            await ReplyAsync(json.ReplyToken, json.MessageText, channelAccessToken);
            return new OkResult();
        }
        return new BadRequestResult();
    }

    private async Task ReplyAsync(string replyToken, string message, string accessToken)
    {
        using var httpClient = httpClientFactory.CreateClient("LineMessagingApi");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.PostAsJsonAsync(_messagingApiEndpoint, new LineTextReplyJson()
        {
            ReplyToken = replyToken,
            Messages = [new() { Type = "text", Text = message }]
        });
        response.EnsureSuccessStatusCode();
    }

    private static bool IsSignature(string signature, string text, string key)
    {
        var textBytes = Encoding.UTF8.GetBytes(text);
        var keyBytes = Encoding.UTF8.GetBytes(key);

        using (HMACSHA256 hmac = new(keyBytes))
        {
            var hash = hmac.ComputeHash(textBytes, 0, textBytes.Length);
            var hash64 = Convert.ToBase64String(hash);

            return signature == hash64;
        }
    }
}
