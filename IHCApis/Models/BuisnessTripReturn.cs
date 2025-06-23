using Newtonsoft.Json;

namespace iHC.Models.Forms
{
    public class BuisnessTripReturn
    {
        [JsonProperty("documentTypeId")]
        public long DocumentTypeId { get; set; }

        [JsonProperty("SystemDocumentType")]
        public string SystemDocumentType { get; set; }

        [JsonProperty("DocumentType")]
        public string DocumentType { get; set; }

        [JsonProperty("DocumentCode")]
        public string DocumentCode { get; set; }

        //[JsonProperty("PersonId")]
        //public long PersonId { get; set; }

        [JsonProperty("PersonNumber")]
        public string PersonNumber { get; set; }

        [JsonProperty("attachments")]
        public List<object> attachments { get; set; }

        [JsonProperty("documentRecordsDFF")]
        public List<BuisnessTripReturnDetails> documentRecordsDFF { get; set; }
    }

    public class BuisnessTripReturnDetails
    {
        [JsonProperty("DocumentsOfRecordId")]
        public long DocumentsOfRecordId { get; set; }


        [JsonProperty("__FLEX_Context")]
        public string __FLEX_Context { get; set; }

        [JsonProperty("__FLEX_Context_DisplayValue")]
        public string __FLEX_Context_DisplayValue { get; set; }

        [JsonProperty("dummy")]
        public string dummy { get; set; }

        //[JsonProperty("BusinessTrip")]
        //public string BusinessTrip { get; set; }

        [JsonProperty("countryFrom")]
        public string countryFrom { get; set; }

        [JsonProperty("cityFrom")]
        public string cityFrom { get; set; }

        [JsonProperty("countryTo")]
        public string countryTo { get; set; }

        [JsonProperty("cityTo")]
        public string cityTo { get; set; }

        [JsonProperty("travelDuration")]
        public int? travelDuration { get; set; }

        [JsonProperty("flyingHours")]
        public int flyingHours { get; set; }

        [JsonProperty("activityStartDate")]
        public string activityStartDate { get; set; }

        [JsonProperty("activityEndDate")]
        public string activityEndDate { get; set; }

        [JsonProperty("employeeCostForHousing")]
        public decimal employeeCostForHousing { get; set; }

        [JsonProperty("wayOfTravel")]
        public string wayOfTravel { get; set; }

        [JsonProperty("otherExpenses")]
        public decimal otherExpenses { get; set; }

        [JsonProperty("otherExpensesDetails")]
        public string otherExpensesDetails { get; set; }

        [JsonProperty("employeeCostForVisa")]
        public decimal? employeeCostForVisa { get; set; }

        [JsonProperty("providedTransportation")]
        public string providedTransportation { get; set; }

        [JsonProperty("visaFees")]
        public decimal visaFees { get; set; }
    }

    public class BuisnessTripReturnResponse
    {
        [JsonProperty("DocumentsOfRecordId")]
        public long DocumentsOfRecordId { get; set; }
    }
}
