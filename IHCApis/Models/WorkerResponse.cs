using System.Text.Json.Serialization;

namespace iHC.Models.Forms
{
    public class WorkerResponse
    {
        [JsonPropertyName("items")]
        public List<WorkerItem> Items { get; set; }
    }

    public class WorkerItem
    {
        [JsonPropertyName("PersonId")]
        public long PersonId { get; set; }

        [JsonPropertyName("names")]
        public List<Name> Names { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("NameLanguage")]
        public string NameLanguage { get; set; }
    }
}
