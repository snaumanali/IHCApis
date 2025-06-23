using System.Text.Json.Serialization;

namespace iHC.Models.Forms
{
    public class AbsenceBalanceRequest
    {
        [JsonPropertyName("personId")]
        public long PersonId { get; set; }

        [JsonPropertyName("typeId")]
        public long TypeId { get; set; }

        [JsonPropertyName("effectiveDate")]
        public string EffectiveDate { get; set; }
    }

    public class AbsenceBalanceResult
    {
        [JsonPropertyName("result")]
        public AbsenceBalanceDetails Result { get; set; }
    }

    public class AbsenceBalanceDetails
    {
        [JsonPropertyName("absenceTypeUOM")]
        public string AbsenceTypeUOM { get; set; }

        [JsonPropertyName("absenceTypeFormattedTotalBalance")]
        public string AbsenceTypeFormattedTotalBalance { get; set; }

        [JsonPropertyName("uomMeaning")]
        public string UomMeaning { get; set; }
    }

}
