namespace iHC.Models.Forms
{
    public class ReportRisksDto
    {
        public int Id { get; set; }
        public string RiskName { get; set; }
        public string RoleDepartment { get; set; }
        public DateTime IdentificationDate { get; set; }
        public string RiskEvent { get; set; }
        public string RiskCauses1 { get; set; }
        public string RiskCauses2 { get; set; }
        public string RiskImpacts1 { get; set; }
        public string RiskImpacts2 { get; set; }
        public string RiskClass { get; set; }
        public string RegulatoryControls { get; set; }
        public string RegulatoryAttachmentsPath { get; set; }
        public string RiskOwnerName { get; set; }
        public string RiskOwnerSignature { get; set; }
        public DateTime RiskOwnerSignatureDate { get; set; }
        public string RiskManagerName { get; set; }
        public string RiskManagerSignature { get; set; }
        public DateTime RiskManagerSignatureDate { get; set; }
        public string InternalAuditName { get; set; }
        public string InternalAuditSignature { get; set; }
        public DateTime InternalAuditSignatureDate { get; set; }
        public string Notes { get; set; }
        public string NoteAttachmentsPath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
