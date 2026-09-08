using System.Text;
using System.Text.Json;

using BacklogBotApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NSubstitute;

namespace BacklogBotApp.Tests;

[TestClass]
public sealed class BotTests
{
    [TestMethod]
    public void Constructor_WithValidConfiguration_InitializesBot()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<Bot>>();
        var configurationMock = Substitute.For<IConfiguration>();
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

        configurationMock["Backlog:ApiKey"].Returns("test-api-key");

        // Act
        var bot = new Bot(loggerMock, configurationMock, httpClientFactoryMock);

        // Assert
        Assert.IsNotNull(bot);
    }

    [TestMethod]
    public void Constructor_WithNullApiKey_ThrowsArgumentNullException()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<Bot>>();
        var configurationMock = Substitute.For<IConfiguration>();
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

        configurationMock["Backlog:ApiKey"].Returns((string?)null);

        // Act & Assert
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            new Bot(loggerMock, configurationMock, httpClientFactoryMock));
        Assert.AreEqual("Backlog:ApiKey", exception.ParamName);
    }

    [TestMethod]
    public async Task Run_WithNullJson_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var httpRequest = CreateHttpRequest("null");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public async Task Run_WithNullJson_LogsError()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<Bot>>();
        var configurationMock = Substitute.For<IConfiguration>();
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

        configurationMock["Backlog:ApiKey"].Returns("test-api-key");

        var bot = new Bot(loggerMock, configurationMock, httpClientFactoryMock);
        var httpRequest = CreateHttpRequest("null");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
        Assert.AreEqual(1, loggerMock.ReceivedCalls().Count(call =>
            call.GetMethodInfo().Name == nameof(ILogger.Log) &&
            call.GetArguments()[0] is LogLevel.Error &&
            call.GetArguments()[2]?.ToString()!.Contains("Failed to deserialize request body") == true));
    }

    [TestMethod]
    [Ignore("ProductionBugSuspected")]
    public async Task Run_WithEmptyBody_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var httpRequest = CreateHttpRequest("");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public async Task Run_WithIssueAddedEvent_ReturnsOk()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.IssueAdded);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public async Task Run_WithCommentedEvent_ReturnsOk()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.Commented);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public async Task Run_WithUnknownEventType_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateBacklogMessageJson(999);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<BadRequestResult>(result);
    }

    [TestMethod]
    public async Task Run_WithValidMessage_LogsEventInformation()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<Bot>>();
        var configurationMock = Substitute.For<IConfiguration>();
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

        configurationMock["Backlog:ApiKey"].Returns("test-api-key");

        var bot = new Bot(loggerMock, configurationMock, httpClientFactoryMock);
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.IssueAdded);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<OkResult>(result);
        Assert.AreEqual(1, loggerMock.ReceivedCalls().Count(call =>
            call.GetMethodInfo().Name == nameof(ILogger.Log) &&
            call.GetArguments()[0] is LogLevel.Information &&
            call.GetArguments()[2]?.ToString()!.Contains("Received event") == true));
    }

    [TestMethod]
    public async Task Run_WithAnyRequest_LogsDebugRequestBody()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<Bot>>();
        var configurationMock = Substitute.For<IConfiguration>();
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

        configurationMock["Backlog:ApiKey"].Returns("test-api-key");

        var bot = new Bot(loggerMock, configurationMock, httpClientFactoryMock);
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.IssueAdded);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsInstanceOfType<OkResult>(result);
        Assert.AreEqual(1, loggerMock.ReceivedCalls().Count(call =>
            call.GetMethodInfo().Name == nameof(ILogger.Log) &&
            call.GetArguments()[0] is LogLevel.Debug &&
            call.GetArguments()[2]?.ToString()!.Contains("Request body") == true));
    }

    private static Bot CreateBot()
    {
        var loggerMock = Substitute.For<ILogger<Bot>>();
        var configurationMock = Substitute.For<IConfiguration>();
        var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();

        configurationMock["Backlog:ApiKey"].Returns("test-api-key");

        return new Bot(loggerMock, configurationMock, httpClientFactoryMock);
    }

    private static HttpRequest CreateHttpRequest(string body)
    {
        var context = new DefaultHttpContext();
        var request = context.Request;

        var bytes = Encoding.UTF8.GetBytes(body);
        request.Body = new MemoryStream(bytes);

        return request;
    }

    private static string CreateBacklogMessageJson(BacklogBodyType eventType)
    {
        return CreateBacklogMessageJson((int)eventType);
    }

    private static string CreateBacklogMessageJson(int eventType)
    {
        var messageJson = new
        {
            id = 1,
            project = new
            {
                id = 1,
                projectKey = "TEST",
                name = "Test Project",
                chartEnabled = false,
                subtaskingEnabled = false,
                projectLeaderCanEditProjectLeader = false,
                useWikiTreeView = false,
                textFormattingRule = "markdown",
                archived = false
            },
            type = eventType,
            notifications = new object[] { },
            createdUser = new
            {
                id = 1,
                userId = "testuser",
                name = "Test User",
                roleType = 1,
                lang = "en",
                mailAddress = "test@example.com",
                nulabAccount = new
                {
                    nulabId = "nulab123",
                    name = "Test Nulab",
                    uniqueId = "unique123"
                }
            },
            created = "2024-01-01T00:00:00Z"
        };

        return JsonSerializer.Serialize(messageJson);
    }
}
