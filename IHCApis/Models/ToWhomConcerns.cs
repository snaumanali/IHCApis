namespace IHCApis.Models
{
    public class ToWhomConcerns
    {
        public List<ToWhomType> Items { get; set; }
    }

    public class ToWhomType
    {
        public string LookupType { get; set; }
        public string ModuleId { get; set; }
        public string Meaning { get; set; }
        public string Description { get; set; }
        public string CustomizationLevel { get; set; }
        public string RestAccessSecured { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public List<ToWhomValue> lookupCodes { get; set; }
    }

    public class ToWhomValue
    {
        public string LookupCode { get; set; }
        public string Meaning { get; set; }
        public string Description { get; set; }
        public string EnabledFlag { get; set; }
        public int DisplaySequence { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public List<Translation> Translations { get; set; }
    }

    public class Translation
    {
        public string Language { get; set; }
        public string SourceLang { get; set; }
        public string Meaning { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
