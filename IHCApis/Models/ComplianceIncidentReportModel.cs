namespace IHCApis.Models
{
    public class CertificateRequestModel
    {
        // Emerging Risk Reporting Form
        public string RiskIdentifier { get; set; }
        public string RoleDepartment { get; set; }
        public DateTime ComplianceIncidentIdentificationDate { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Information of Violator
        public string ViolatorName { get; set; }
        public string ViolatorJobTitle { get; set; }
        public string ViolatorDepartment { get; set; }
        public string Participants { get; set; }

        // Regulatory Controls Section
        public DateTime IncidentDate { get; set; }
        public DateTime NoticingDate { get; set; }
        public string IncidentLocation { get; set; }
        public string IncidentDetails { get; set; }
        public string Witnesses { get; set; }
        public string Comments { get; set; }

        // Evidence & Supporting Documents
        public string EvidenceFile { get; set; }

        // Reporting Section
        public string HasReportedBefore { get; set; }
        public string NotifiedParty { get; set; }
    }
}
