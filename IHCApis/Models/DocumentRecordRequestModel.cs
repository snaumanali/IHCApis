using System.Text.Json.Serialization;

namespace IHCApis.Models
{
    public class DocumentRecordsDff
    {
        [JsonPropertyName("__FLEX_Context")]
        public string FlexContext { get; set; } = "SA_EMPLOYMENT_CERTIFICATE";

        [JsonPropertyName("__FLEX_Context_DisplayValue")]
        public string FlexContextDisplayValue { get; set; } = "Employment Certificate";

        [JsonPropertyName("requestDate")]
        public string RequestDate { get; set; } = "2025-04-20";

        [JsonPropertyName("toWhomItMayConcern")]
        public string ToWhomItMayConcern { get; set; } = "ANB_BANK";

        [JsonPropertyName("certificateType")]
        public string CertificateType { get; set; } = "Employment Letter";

        [JsonPropertyName("reason")]
        public string? Reason { get; set; } = null;
      
    }

    public class DocumentRecordRequestModel
    {
        [JsonPropertyName("DocumentTypeId")]
        public long DocumentTypeId { get; set; } = 300000002112913;

        [JsonPropertyName("SystemDocumentType")]
        public string SystemDocumentType { get; set; } = "SA_EMPLOYMENT_CERTIFICATE";

        [JsonPropertyName("DocumentType")]
        public string DocumentType { get; set; } = "Employment Certificate";

        [JsonPropertyName("DocumentCode")]
        public string DocumentCode { get; set; } = "SA_EMPLOYMENT_CERTIFICATE_2025-04-27-11-59-09-950000";

        [JsonPropertyName("PersonId")]
        public long PersonId { get; set; } = 300000004659857;

        [JsonPropertyName("PersonNumber")]
        public string PersonNumber { get; set; } = "79";

        [JsonPropertyName("documentRecordsDFF")]
        public List<DocumentRecordsDff> DocumentRecordsDFF { get; set; }

        [JsonPropertyName("attachments")]
        public List<Attachment> Attachments { get; set; }
    }
}
