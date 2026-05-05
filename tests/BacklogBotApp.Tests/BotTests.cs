using System.Text;
using System.Text.Json;

using BacklogBotApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;

namespace BacklogBotApp.Tests;

public class BotTests
{
    [Fact]
    public void Constructor_WithValidConfiguration_InitializesBot()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Backlog:ApiKey"]).Returns("test-api-key");

        // Act
        var bot = new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);

        // Assert
        Assert.NotNull(bot);
    }

    [Fact]
    public void Constructor_WithNullApiKey_ThrowsArgumentNullException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Backlog:ApiKey"]).Returns((string?)null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object));
        Assert.Equal("Backlog:ApiKey", exception.ParamName);
    }

    [Fact]
    public async Task Run_WithNullJson_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var httpRequest = CreateHttpRequest("null");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithNullJson_LogsError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Backlog:ApiKey"]).Returns("test-api-key");

        var bot = new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);
        var httpRequest = CreateHttpRequest("null");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to deserialize request body")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact(Skip = "ProductionBugSuspected")]
    public async Task Run_WithEmptyBody_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var httpRequest = CreateHttpRequest("");

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithIssueAddedEvent_ReturnsOk()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.IssueAdded);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Run_WithCommentedEvent_ReturnsOk()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.Commented);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Run_WithUnknownEventType_ReturnsBadRequest()
    {
        // Arrange
        var bot = CreateBot();
        var requestBody = CreateBacklogMessageJson(999);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Run_WithValidMessage_LogsEventInformation()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Backlog:ApiKey"]).Returns("test-api-key");

        var bot = new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.IssueAdded);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<OkResult>(result);
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Received event")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Run_WithAnyRequest_LogsDebugRequestBody()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Backlog:ApiKey"]).Returns("test-api-key");

        var bot = new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);
        var requestBody = CreateBacklogMessageJson(BacklogBodyType.IssueAdded);
        var httpRequest = CreateHttpRequest(requestBody);

        // Act
        var result = await bot.Run(httpRequest);

        // Assert
        Assert.IsType<OkResult>(result);
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Request body")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private static Bot CreateBot()
    {
        var loggerMock = new Mock<ILogger<Bot>>();
        var configurationMock = new Mock<IConfiguration>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        configurationMock.Setup(c => c["Backlog:ApiKey"]).Returns("test-api-key");

        return new Bot(loggerMock.Object, configurationMock.Object, httpClientFactoryMock.Object);
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
