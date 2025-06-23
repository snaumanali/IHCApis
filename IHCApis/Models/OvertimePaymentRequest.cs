using System.Text.Json.Serialization;

namespace iHC.Models.Forms
{
    public class OvertimePaymentRequest
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

        [JsonPropertyName("documentRecordsDFF")]
        public List<DocumentRecord> DocumentRecordsDFF { get; set; }

        [JsonPropertyName("attachments")]
        public List<Attachment> Attachments { get; set; }
    }

    public class DocumentRecord
    {
        [JsonPropertyName("__FLEX_Context")]
        public string __FLEX_Context { get; set; }

        [JsonPropertyName("__FLEX_Context_DisplayValue")]
        public string __FLEX_Context_DisplayValue { get; set; }

        [JsonPropertyName("dummy")]
        public string Dummy { get; set; }

        [JsonPropertyName("requestDate")]
        public string RequestDate { get; set; }

        [JsonPropertyName("overtimeAssignment")]
        public string OvertimeAssignment { get; set; }

        [JsonPropertyName("actualNoOfHours")]
        public int ActualNoOfHours { get; set; }

        [JsonPropertyName("justfications")]
        public string Justfications { get; set; }
    }

    public class Attachment
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("FileContents")]
        public string FileContents { get; set; } // Assuming file contents are base64 encoded
    }
}
