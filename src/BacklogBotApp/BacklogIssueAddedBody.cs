using System.Text.Json.Serialization;

namespace BacklogBotApp;

public class BacklogIssueAddedBody : BacklogBody
{
    [JsonPropertyName("content")]
    public ContentObject Content { get; set; }

    public class ContentObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("key_id")]
        public int KeyId { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("issueType")]
        public IssueTypeObject IssueType { get; set; }
        [JsonPropertyName("resolution")]
        public object Resolution { get; set; }
        [JsonPropertyName("priority")]
        public PriorityObject Priority { get; set; }
        [JsonPropertyName("status")]
        public StatusObject Status { get; set; }
        [JsonPropertyName("assignee")]
        public object Assignee { get; set; }
        [JsonPropertyName("category")]
        public object[] Category { get; set; }
        [JsonPropertyName("versions")]
        public object[] Versions { get; set; }
        [JsonPropertyName("milestone")]
        public object[] Milestone { get; set; }
        [JsonPropertyName("startDate")]
        public object StartDate { get; set; }
        [JsonPropertyName("dueDate")]
        public object DueDate { get; set; }
        [JsonPropertyName("estimatedHours")]
        public object EstimatedHours { get; set; }
        [JsonPropertyName("actualHours")]
        public object ActualHours { get; set; }
        [JsonPropertyName("parentIssueId")]
        public object ParentIssueId { get; set; }
        [JsonPropertyName("customFields")]
        public object[] CustomFields { get; set; }
        [JsonPropertyName("attachments")]
        public object[] Attachments { get; set; }
        [JsonPropertyName("shared_files")]
        public object[] SharedFiles { get; set; }
        [JsonPropertyName("externalFileLinks")]
        public object[] ExternalFileLinks { get; set; }
    }

    public class IssueTypeObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("projectId")]
        public int ProjectId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
        [JsonPropertyName("displayOrder")]
        public int DisplayOrder { get; set; }
    }

    public class PriorityObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class StatusObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}