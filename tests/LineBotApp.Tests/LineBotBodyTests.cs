using LineBotApp;
using Xunit;

namespace LineBotApp.Tests;

public class LineBotBodyTests
{
    [Fact]
    public void FirstMessage_WhenEventsIsNull_ReturnsNull()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.FirstMessage;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FirstMessage_WhenEventsIsEmpty_ReturnsNull()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.FirstMessage;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FirstMessage_WhenFirstEventHasNoMessage_ReturnsNull()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Message = null }
            }
        };

        // Act
        var result = lineMessage.FirstMessage;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FirstMessage_WhenFirstEventHasMessage_ReturnsMessage()
    {
        // Arrange
        var message = new Message { Id = "123", Type = "text", Text = "Hello" };
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Message = message }
            }
        };

        // Act
        var result = lineMessage.FirstMessage;

        // Assert
        Assert.Same(message, result);
    }

    [Fact]
    public void MessageText_WhenEventsIsNull_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.MessageText;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void MessageText_WhenEventsIsEmpty_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.MessageText;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void MessageText_WhenFirstEventHasNoMessage_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Message = null }
            }
        };

        // Act
        var result = lineMessage.MessageText;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void MessageText_WhenMessageHasText_ReturnsText()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message { Text = "Hello World" }
                }
            }
        };

        // Act
        var result = lineMessage.MessageText;

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void MessageText_WhenMessageTextIsEmpty_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message { Text = string.Empty }
                }
            }
        };

        // Act
        var result = lineMessage.MessageText;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReplyToken_WhenEventsIsNull_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.ReplyToken;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReplyToken_WhenEventsIsEmpty_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.ReplyToken;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReplyToken_WhenFirstEventHasNullReplyToken_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { ReplyToken = null }
            }
        };

        // Act
        var result = lineMessage.ReplyToken;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReplyToken_WhenFirstEventHasReplyToken_ReturnsReplyToken()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { ReplyToken = "token123" }
            }
        };

        // Act
        var result = lineMessage.ReplyToken;

        // Assert
        Assert.Equal("token123", result);
    }

    [Fact]
    public void ReplyToken_WhenFirstEventHasEmptyReplyToken_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { ReplyToken = string.Empty }
            }
        };

        // Act
        var result = lineMessage.ReplyToken;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EventType_WhenEventsIsNull_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.EventType;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EventType_WhenEventsIsEmpty_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.EventType;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EventType_WhenFirstEventHasNullType_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = null }
            }
        };

        // Act
        var result = lineMessage.EventType;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void EventType_WhenFirstEventHasType_ReturnsType()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = "message" }
            }
        };

        // Act
        var result = lineMessage.EventType;

        // Assert
        Assert.Equal("message", result);
    }

    [Fact]
    public void EventType_WhenFirstEventHasEmptyType_ReturnsEmptyString()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = string.Empty }
            }
        };

        // Act
        var result = lineMessage.EventType;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventsIsNull_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventsIsEmpty_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventTypeIsNull_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = null }
            }
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventTypeIsMessage_ReturnsTrue()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = "message" }
            }
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventTypeIsNotMessage_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = "follow" }
            }
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventTypeIsEmptyString_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = string.Empty }
            }
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsMessageEvent_WhenEventTypeIsDifferentCase_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Type = "MESSAGE" }
            }
        };

        // Act
        var result = lineMessage.IsMessageEvent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsTextType_WhenFirstMessageIsNull_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsTextType_WhenEventsIsEmpty_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsTextType_WhenFirstEventHasNoMessage_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Message = null }
            }
        };

        // Act
        var result = lineMessage.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsTextType_WhenMessageTypeIsText_ReturnsTrue()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message { Type = "text" }
                }
            }
        };

        // Act
        var result = lineMessage.IsTextType;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsTextType_WhenMessageTypeIsNotText_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message { Type = "image" }
                }
            }
        };

        // Act
        var result = lineMessage.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsTextType_WhenMessageTypeIsEmpty_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message { Type = string.Empty }
                }
            }
        };

        // Act
        var result = lineMessage.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageType_WhenFirstMessageIsNull_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = null
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageType_WhenEventsIsEmpty_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>()
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageType_WhenFirstEventHasNoMessage_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event { Message = null }
            }
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageType_WhenMessageTypeIsImageAndContentProviderIsLine_ReturnsTrue()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message
                    {
                        Type = "image",
                        ContentProvider = new ContentProvider { Type = "line" }
                    }
                }
            }
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsImageType_WhenMessageTypeIsImageButContentProviderIsNull_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message
                    {
                        Type = "image",
                        ContentProvider = null
                    }
                }
            }
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageType_WhenMessageTypeIsImageButContentProviderTypeIsNotLine_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message
                    {
                        Type = "image",
                        ContentProvider = new ContentProvider { Type = "external" }
                    }
                }
            }
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImageType_WhenMessageTypeIsNotImage_ReturnsFalse()
    {
        // Arrange
        var lineMessage = new LineMessageReceiveJson
        {
            Events = new List<Event>
            {
                new Event
                {
                    Message = new Message
                    {
                        Type = "text",
                        ContentProvider = new ContentProvider { Type = "line" }
                    }
                }
            }
        };

        // Act
        var result = lineMessage.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsTextType_WhenTypeIsText_ReturnsTrue()
    {
        // Arrange
        var message = new Message { Type = "text" };

        // Act
        var result = message.IsTextType;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Message_IsTextType_WhenTypeIsNotText_ReturnsFalse()
    {
        // Arrange
        var message = new Message { Type = "image" };

        // Act
        var result = message.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsTextType_WhenTypeIsEmpty_ReturnsFalse()
    {
        // Arrange
        var message = new Message { Type = string.Empty };

        // Act
        var result = message.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsTextType_WhenTypeIsDifferentCase_ReturnsFalse()
    {
        // Arrange
        var message = new Message { Type = "TEXT" };

        // Act
        var result = message.IsTextType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsImageType_WhenTypeIsImageAndContentProviderTypeIsLine_ReturnsTrue()
    {
        // Arrange
        var message = new Message
        {
            Type = "image",
            ContentProvider = new ContentProvider { Type = "line" }
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Message_IsImageType_WhenTypeIsImageButContentProviderIsNull_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Type = "image",
            ContentProvider = null
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsImageType_WhenTypeIsImageButContentProviderTypeIsNotLine_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Type = "image",
            ContentProvider = new ContentProvider { Type = "external" }
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsImageType_WhenTypeIsNotImage_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Type = "text",
            ContentProvider = new ContentProvider { Type = "line" }
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsImageType_WhenTypeIsEmpty_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Type = string.Empty,
            ContentProvider = new ContentProvider { Type = "line" }
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsImageType_WhenTypeIsDifferentCase_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Type = "IMAGE",
            ContentProvider = new ContentProvider { Type = "line" }
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Message_IsImageType_WhenContentProviderTypeIsEmpty_ReturnsFalse()
    {
        // Arrange
        var message = new Message
        {
            Type = "image",
            ContentProvider = new ContentProvider { Type = string.Empty }
        };

        // Act
        var result = message.IsImageType;

        // Assert
        Assert.False(result);
    }
}
