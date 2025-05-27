using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace IHCApis.Models
{
    public class AbsenceRequest
    {
        [JsonPropertyName("personNumber")]
        public int PersonNumber { get; set; }

        [JsonPropertyName("employer")]
        public string Employer { get; set; }

        [JsonPropertyName("absenceType")]
        public string AbsenceType { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }      // "yyyy-MM-dd"

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }      // "HH:mm"

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }        // "yyyy-MM-dd"

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }        // "HH:mm"

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }  // e.g. "D"

        [JsonPropertyName("absenceStatusCd")]
        public string AbsenceStatusCd { get; set; }   // e.g. "SUBMITTED"

        [JsonPropertyName("approvalStatusCd")]
        public string ApprovalStatusCd { get; set; }  // e.g. "AWAITING"

        [JsonPropertyName("singleDayFlag")]
        public string SingleDayFlag { get; set; }     // "Y" or "N"

        [JsonPropertyName("startDateDuration")]
        public int StartDateDuration { get; set; }
    }
}
