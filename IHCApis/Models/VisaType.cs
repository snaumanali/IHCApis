using System.Text.Json.Serialization;

namespace IHCApis.Models
{

    public class VisaType
    {
        [JsonPropertyName("items")]
        public List<LookupItems> Items { get; set; }
    }

    public class LookupItems
    {
        [JsonPropertyName("lookupCodes")]
        public List<LookupCodes> LookupCodes { get; set; }
    }

    public class LookupCodes
    {
        [JsonPropertyName("LookupCode")]
        public string LookupCode { get; set; }

        [JsonPropertyName("Meaning")]
        public string Meaning { get; set; }

        [JsonPropertyName("EnabledFlag")]
        public string EnabledFlag { get; set; }

        [JsonPropertyName("translations")]
        public List<Translations> Translations { get; set; }
    }

    public class Translations
    {
        [JsonPropertyName("Language")]
        public string Language { get; set; }

        [JsonPropertyName("Meaning")]
        public string Meaning { get; set; }
    }

}
