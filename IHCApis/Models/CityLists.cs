using System.Text.Json.Serialization;

namespace iHC.Models.Forms
{
    public class CityLists
    {
        [JsonPropertyName("items")]
        public List<ValueSetItem> Items { get; set; }
    }

    public class ValueSetItem
    {
        [JsonPropertyName("values")]
        public List<ValueEntry> Values { get; set; }
    }

    public class ValueEntry
    {
        [JsonPropertyName("Value")]
        public string Value { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("EnabledFlag")]
        public string EnabledFlag { get; set; }
    }
}
