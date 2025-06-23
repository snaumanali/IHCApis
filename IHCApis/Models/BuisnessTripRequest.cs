using System.Text.Json.Serialization;

namespace iHC.Models.Forms
{
    public class BusinessTripRequest
    {
        [JsonPropertyName("DocumentTypeId")]
        public long DocumentTypeId { get; set; }

        [JsonPropertyName("SystemDocumentType")]
        public string SystemDocumentType { get; set; }

        [JsonPropertyName("DocumentType")]
        public string DocumentType { get; set; }

        [JsonPropertyName("DocumentCode")]
        public string DocumentCode { get; set; }

        [JsonPropertyName("PersonId")]
        public long PersonId { get; set; }

        [JsonPropertyName("PersonNumber")]
        public string PersonNumber { get; set; }

        [JsonPropertyName("attachments")]
        public List<object> Attachments { get; set; } = new();

        [JsonPropertyName("documentRecordsDFF")]
        public List<DocumentRecordDff> DocumentRecordsDFF { get; set; }
    }

    public class DocumentRecordDff
    {
        [JsonPropertyName("DocumentsOfRecordId")]
        public long DocumentsOfRecordId { get; set; }

        [JsonPropertyName("__FLEX_Context")]
        public string FlexContext { get; set; }

        [JsonPropertyName("__FLEX_Context_DisplayValue")]
        public string FlexContextDisplayValue { get; set; }

        [JsonPropertyName("dummy")]
        public string Dummy { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("businessTripType")]
        public string BusinessTripType { get; set; }

        [JsonPropertyName("businessTripType_Display")]
        public string BusinessTripTypeDisplay { get; set; }

        [JsonPropertyName("countryFrom")]
        public string CountryFrom { get; set; }

        [JsonPropertyName("countryFrom_Display")]
        public string CountryFromDisplay { get; set; }

        [JsonPropertyName("cityFrom")]
        public string CityFrom { get; set; }

        [JsonPropertyName("fromCity")]
        public string FromCity { get; set; }

        [JsonPropertyName("countryTo")]
        public string CountryTo { get; set; }

        [JsonPropertyName("countryTo_Display")]
        public string CountryToDisplay { get; set; }

        [JsonPropertyName("cityTo")]
        public string CityTo { get; set; }

        [JsonPropertyName("toCity")]
        public string ToCity { get; set; }

        [JsonPropertyName("travelDuration")]
        public int? TravelDuration { get; set; }

        [JsonPropertyName("activityStartDate")]
        public string ActivityStartDate { get; set; }

        [JsonPropertyName("activityEndDate")]
        public string ActivityEndDate { get; set; }

        [JsonPropertyName("travelDate")]
        public string TravelDate { get; set; }

        [JsonPropertyName("wayOfTravel")]
        public string WayOfTravel { get; set; }

        [JsonPropertyName("providedTransportation")]
        public string ProvidedTransportation { get; set; }

        [JsonPropertyName("inAdvancePayment")]
        public string InAdvancePayment { get; set; }

        [JsonPropertyName("theSubstituteEmployee")]
        public string TheSubstituteEmployee { get; set; }

        [JsonPropertyName("travelingWith")]
        public string TravelingWith { get; set; }

        [JsonPropertyName("trainingNumber")]
        public string TrainingNumber { get; set; }

        [JsonPropertyName("courseName")]
        public string CourseName { get; set; }

        [JsonPropertyName("exitReEntry")]
        public string ExitReEntry { get; set; }

        [JsonPropertyName("visaRequired")]
        public string VisaRequired { get; set; }
    }

    public class DocumentRecordResponse
    {
        [JsonPropertyName("DocumentsOfRecordId")]
        public long DocumentsOfRecordId { get; set; }
    }
}
