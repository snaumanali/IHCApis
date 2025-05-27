namespace IHCApis.Models
{
    public class Dependents
    {
        public List<HcmContactItem> Items { get; set; }
        public int Count { get; set; }
        public bool HasMore { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    public class HcmContactItem
    {
        public List<Name> Names { get; set; }
        public List<ContactRelationship> ContactRelationships { get; set; }
        public List<Phone> Phones { get; set; }
    }

    public class Name
    {
        public string DisplayName { get; set; }
    }

    public class ContactRelationship
    {
        public string ContactTypeMeaning { get; set; }
    }

    public class Phone
    {
        public long PhoneId { get; set; }
        public string PhoneType { get; set; }
        public string PhoneTypeMeaning { get; set; }
        public string LegislationCode { get; set; }
        public string LegislationName { get; set; }
        public string CountryCodeNumber { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Validity { get; set; }
        public string ValidityMeaning { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdateDate { get; set; }
        public bool PrimaryFlag { get; set; }
    }

}
