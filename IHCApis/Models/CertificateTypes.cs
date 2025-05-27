namespace IHCApis.Models
{
    public class CertificateTypes
    {
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public List<ValueItem> Values { get; set; }
    }

    public class ValueItem
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public string EnabledFlag { get; set; }
    }
}
