namespace BacklogBotApp;

public class BacklogBody
{
    public int id { get; set; }
    public Project project { get; set; }
    public BacklogBodyType type { get; set; }
    public object[] notifications { get; set; }
    public Createduser createdUser { get; set; }
    public DateTime created { get; set; }

    public class Project
    {
        public int id { get; set; }
        public string projectKey { get; set; }
        public string name { get; set; }
        public bool chartEnabled { get; set; }
        public bool subtaskingEnabled { get; set; }
        public bool projectLeaderCanEditProjectLeader { get; set; }
        public bool useWikiTreeView { get; set; }
        public string textFormattingRule { get; set; }
        public bool archived { get; set; }
    }

    public class Createduser
    {
        public int id { get; set; }
        public object userId { get; set; }
        public string name { get; set; }
        public int roleType { get; set; }
        public string lang { get; set; }
        public object mailAddress { get; set; }
        public Nulabaccount nulabAccount { get; set; }
    }

    public class Nulabaccount
    {
        public string nulabId { get; set; }
        public string name { get; set; }
        public string uniqueId { get; set; }
    }
}
