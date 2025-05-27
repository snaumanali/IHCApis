namespace IHCApis.Models
{

    public class WayOfTravel
    {
        public List<ValueSetItem> Items { get; set; }
    }

    public class ValueSetItems
    {
        public string ValueSetCode { get; set; }
        public List<ValueItem> Values { get; set; }
    }

    public class ValueItems
    {
        public string Value { get; set; }
    }
}
