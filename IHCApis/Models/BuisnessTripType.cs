using Newtonsoft.Json;

namespace IHCApis.Models
{
    public class BuisnessTripType
    {
        [JsonProperty("items")]
        public List<LookupItem> Items { get; set; }
    }

    public class LookupItem
    {
        [JsonProperty("lookupCodes")]
        public List<LookupDetails> LookupCodes { get; set; }
    }

    public class LookupDetails
    {
        [JsonProperty("LookupCode")]
        public string LookupCode { get; set; }

        [JsonProperty("Meaning")]
        public string Meaning { get; set; }

        [JsonProperty("EnabledFlag")]
        public string EnabledFlag { get; set; }
    }

}
