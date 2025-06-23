namespace iHC.Models.Forms
{
    public class CertificateRequestDto
    {
        public int Id { get; set; }
        public string RiskIdentifier { get; set; }
        public string RoleDepartment { get; set; }
        public DateTime ComplianceIncidentIdentificationDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ViolatorName { get; set; }
        public string ViolatorJobTitle { get; set; }
        public string ViolatorDepartment { get; set; }
        public string Participants { get; set; }
        public DateTime IncidentDate { get; set; }
        public DateTime NoticingDate { get; set; }
        public string IncidentLocation { get; set; }
        public string IncidentDetails { get; set; }
        public string Witnesses { get; set; }
        public string Comments { get; set; }
        public string EvidenceFile { get; set; }
        public string HasReportedBefore { get; set; }
        public string NotifiedParty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
