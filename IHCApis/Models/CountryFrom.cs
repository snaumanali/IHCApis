using System.Text.Json.Serialization;

namespace IHCApis.Models
{
    public class CountryFrom
    {
        [JsonPropertyName("items")]
        public List<UserTableDetailItem> Items { get; set; }
    }

    public class UserTableDetailItem
    {
        [JsonPropertyName("UserTableId")]
        public long UserTableId { get; set; }

        [JsonPropertyName("UserRowId")]
        public long UserRowId { get; set; }

        [JsonPropertyName("RowName")]
        public string RowName { get; set; }

        [JsonPropertyName("DisplaySequence")]
        public int DisplaySequence { get; set; }

        // … you can include other columns if you need them
    }
}
