using Newtonsoft.Json;

namespace IHCApis.Models
{
    public class BuisnessTripReturn
    {
        [JsonProperty("DocumentTypeId")]
        public long DocumentTypeId { get; set; }

        [JsonProperty("SystemDocumentType")]
        public string SystemDocumentType { get; set; }

        [JsonProperty("DocumentType")]
        public string DocumentType { get; set; }

        [JsonProperty("DocumentCode")]
        public string DocumentCode { get; set; }

        [JsonProperty("PersonId")]
        public long PersonId { get; set; }

        [JsonProperty("PersonNumber")]
        public string PersonNumber { get; set; }

        [JsonProperty("Attachments")]
        public List<object> Attachments { get; set; }

        [JsonProperty("DocumentRecordsDFF")]
        public List<BuisnessTripReturnDetails> DocumentRecordsDFF { get; set; }
    }

    public class BuisnessTripReturnDetails
    {
        [JsonProperty("DocumentsOfRecordId")]
        public long DocumentsOfRecordId { get; set; }

        [JsonProperty("__FLEX_Context")]
        public string FLEX_Context { get; set; }

        [JsonProperty("__FLEX_Context_DisplayValue")]
        public string FLEX_Context_DisplayValue { get; set; }

        [JsonProperty("Dummy")]
        public string Dummy { get; set; }

        [JsonProperty("BusinessTrip")]
        public string BusinessTrip { get; set; }

        [JsonProperty("CountryFrom")]
        public string CountryFrom { get; set; }

        [JsonProperty("CityFrom")]
        public string CityFrom { get; set; }

        [JsonProperty("CountryTo")]
        public string CountryTo { get; set; }

        [JsonProperty("CityTo")]
        public string CityTo { get; set; }

        [JsonProperty("TravelDuration")]
        public int? TravelDuration { get; set; }

        [JsonProperty("FlyingHours")]
        public int FlyingHours { get; set; }

        [JsonProperty("ActivityStartDate")]
        public string ActivityStartDate { get; set; }

        [JsonProperty("ActivityEndDate")]
        public string ActivityEndDate { get; set; }

        [JsonProperty("EmployeeCostForHousing")]
        public decimal EmployeeCostForHousing { get; set; }

        [JsonProperty("WayOfTravel")]
        public string WayOfTravel { get; set; }

        [JsonProperty("OtherExpenses")]
        public decimal OtherExpenses { get; set; }

        [JsonProperty("OtherExpensesDetails")]
        public string OtherExpensesDetails { get; set; }

        [JsonProperty("EmployeeCostForVisa")]
        public decimal? EmployeeCostForVisa { get; set; }

        [JsonProperty("ProvidedTransportation")]
        public string ProvidedTransportation { get; set; }

        [JsonProperty("VisaFees")]
        public decimal VisaFees { get; set; }
    }

    public class BuisnessTripReturnResponse
    {
        [JsonProperty("DocumentsOfRecordId")]
        public long DocumentsOfRecordId { get; set; }
    }
}
