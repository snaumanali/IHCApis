using System.Text.Json.Serialization;

namespace IHCApis.Models
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
        public List<Names> Names { get; set; }
    }

    public class Names
    {
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("NameLanguage")]
        public string NameLanguage { get; set; }
    }
}
