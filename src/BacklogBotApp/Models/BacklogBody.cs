using System.Text.Json.Serialization;

namespace BacklogBotApp.Models;

public class BacklogBody
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("project")]
    public ProjectObject Project { get; set; }
    [JsonPropertyName("type")]
    public BacklogBodyType Type { get; set; }
    [JsonPropertyName("notifications")]
    public object[] Notifications { get; set; }
    [JsonPropertyName("createdUser")]
    public CreatedUserObject CreatedUser { get; set; }
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    public class ProjectObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("projectKey")]
        public string ProjectKey { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("chartEnabled")]
        public bool ChartEnabled { get; set; }
        [JsonPropertyName("subtaskingEnabled")]
        public bool SubtaskingEnabled { get; set; }
        [JsonPropertyName("projectLeaderCanEditProjectLeader")]
        public bool ProjectLeaderCanEditProjectLeader { get; set; }
        [JsonPropertyName("useWikiTreeView")]
        public bool UseWikiTreeView { get; set; }
        [JsonPropertyName("textFormattingRule")]
        public string TextFormattingRule { get; set; }
        [JsonPropertyName("archived")]
        public bool Archived { get; set; }
    }

    public class CreatedUserObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userId")]
        public object UserId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("roleType")]
        public int RoleType { get; set; }
        [JsonPropertyName("lang")]
        public string Lang { get; set; }
        [JsonPropertyName("mailAddress")]
        public object MailAddress { get; set; }
        [JsonPropertyName("nulabAccount")]
        public NulabAccountObject NulabAccount { get; set; }
    }

    public class NulabAccountObject
    {
        [JsonPropertyName("nulabId")]
        public string NulabId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("uniqueId")]
        public string UniqueId { get; set; }
    }
}
