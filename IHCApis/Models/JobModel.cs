namespace IHCApis.Models
{
    public class JobModel
    {
        public long JobId { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
    }

    public class JobApiResponse
    {
        public List<JobModel> Items { get; set; }
        public int Count { get; set; }
        public bool HasMore { get; set; }
    }

}
