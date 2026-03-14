using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;

namespace LineBotApp.Tests;

public class BotTests
{
    [Fact]
    public void Constructor_WithValidConfiguration_InitializesBot()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Line:AccessToken"]).Returns("test-access-token");
        configurationMock.Setup(c => c["Line:ChannelSecret"]).Returns("test-channel-secret");

        // Act
        var bot = new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);

        // Assert
        Assert.NotNull(bot);
    }

    [Fact]
    public void Constructor_WithNullAccessToken_ThrowsArgumentNullException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Line:AccessToken"]).Returns((string?)null);
        configurationMock.Setup(c => c["Line:ChannelSecret"]).Returns("test-channel-secret");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object));
        Assert.Equal("Line:AccessToken", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullChannelSecret_ThrowsArgumentNullException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Line:AccessToken"]).Returns("test-access-token");
        configurationMock.Setup(c => c["Line:ChannelSecret"]).Returns((string?)null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object));
        Assert.Equal("Line:ChannelSecret", exception.ParamName);
    }

    [Fact]
    public async Task Run_WithMissingXLineSignature_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var httpRequest = CreateHttpRequest(new Dictionary<string, StringValues>(), "{}");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithEmptyXLineSignature_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var headers = new Dictionary<string, StringValues>
        {
            ["X-Line-Signature"] = string.Empty
        };
        var httpRequest = CreateHttpRequest(headers, "{}");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithNullJson_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var headers = new Dictionary<string, StringValues>
        {
            ["X-Line-Signature"] = "test-signature"
        };
        var httpRequest = CreateHttpRequest(headers, "null");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithInvalidSignature_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateValidLineMessageJson();
        var headers = new Dictionary<string, StringValues>
        {
            ["X-Line-Signature"] = "invalid-signature"
        };
        var httpRequest = CreateHttpRequest(headers, requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithValidSignatureButNotMessageEvent_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateLineMessageJson("follow");
        var signature = GenerateValidSignature(requestBody, "test-channel-secret");
        var headers = new Dictionary<string, StringValues>
        {
            ["X-Line-Signature"] = signature
        };
        var httpRequest = CreateHttpRequest(headers, requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithValidSignatureAndMessageEvent_ReturnsOk()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Line:AccessToken"]).Returns("test-access-token");
        configurationMock.Setup(c => c["Line:ChannelSecret"]).Returns("test-channel-secret");

        var messageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(messageHandler)
        {
            BaseAddress = new Uri("https://api.line.me")
        };

        httpClientFactoryMock.Setup(f => f.CreateClient("LineMessagingApi"))
            .Returns(httpClient);

        var bot = new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);

        var requestBody = CreateValidLineMessageJson();
        var signature = GenerateValidSignature(requestBody, "test-channel-secret");
        var headers = new Dictionary<string, StringValues>
        {
            ["X-Line-Signature"] = signature
        };
        var httpRequest = CreateHttpRequest(headers, requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    private static Bot CreateBot()
    {
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Line:AccessToken"]).Returns("test-access-token");
        configurationMock.Setup(c => c["Line:ChannelSecret"]).Returns("test-channel-secret");

        return new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);
    }

    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            return Task.FromResult(response);
        }
    }

    private static HttpRequest CreateHttpRequest(Dictionary<string, StringValues> headers, string body)
    {
        var context = new DefaultHttpContext();
        var request = context.Request;

        foreach (var header in headers)
        {
            request.Headers[header.Key] = header.Value;
        }

        var bytes = Encoding.UTF8.GetBytes(body);
        request.Body = new MemoryStream(bytes);

        return request;
    }

    private static string CreateValidLineMessageJson()
    {
        return CreateLineMessageJson("message");
    }

    private static string CreateLineMessageJson(string eventType)
    {
        var messageJson = new
        {
            destination = "test-destination",
            events = new[]
            {
                new
                {
                    replyToken = "test-reply-token",
                    type = eventType,
                    timestamp = 1234567890,
                    source = new { type = "user", userId = "test-user-id" },
                    message = new { id = "test-message-id", type = "text", text = "Hello" }
                }
            }
        };

        return JsonSerializer.Serialize(messageJson);
    }

    private static string GenerateValidSignature(string text, string key)
    {
        var textBytes = Encoding.UTF8.GetBytes(text);
        var keyBytes = Encoding.UTF8.GetBytes(key);

        using var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(textBytes, 0, textBytes.Length);
        return Convert.ToBase64String(hash);
    }
}
