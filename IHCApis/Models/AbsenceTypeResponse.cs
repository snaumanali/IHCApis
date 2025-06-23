using System.Text.Json.Serialization;

namespace iHC.Models.Forms
{
    public class AbsenceTypeDto
    {
        [JsonPropertyName("AbsenceTypeId")]
        public long AbsenceTypeId { get; set; }
        [JsonPropertyName("AbsenceTypeName")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }

    public class AbsenceTypeResponse
    {
        [JsonPropertyName("items")]
        public List<AbsenceTypeDto> Items { get; set; }
    }
}
