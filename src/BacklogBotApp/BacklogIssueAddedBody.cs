namespace BacklogBotApp;

public class BacklogIssueAddedBody : BacklogBody
{
    public Content content { get; set; }

    public class Content
    {
        public int id { get; set; }
        public int key_id { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public Issuetype issueType { get; set; }
        public object resolution { get; set; }
        public Priority priority { get; set; }
        public Status status { get; set; }
        public object assignee { get; set; }
        public object[] category { get; set; }
        public object[] versions { get; set; }
        public object[] milestone { get; set; }
        public object startDate { get; set; }
        public object dueDate { get; set; }
        public object estimatedHours { get; set; }
        public object actualHours { get; set; }
        public object parentIssueId { get; set; }
        public object[] customFields { get; set; }
        public object[] attachments { get; set; }
        public object[] shared_files { get; set; }
        public object[] externalFileLinks { get; set; }
    }

    public class Issuetype
    {
        public int id { get; set; }
        public int projectId { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public int displayOrder { get; set; }
    }

    public class Priority
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}