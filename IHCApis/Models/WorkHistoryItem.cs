namespace iHC.Models.Forms
{
    public class WorkHistoryItem
    {
        public string JobTitle { get; set; }
        public string EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class WorkHistorySection
    {
        public List<WorkHistoryItem> WorkHistoryItems { get; set; }
    }

    public class TalentProfile
    {
        public string DisplayName { get; set; }
        public string PersonNumber { get; set; }
        public List<WorkHistorySection> WorkHistorySections { get; set; }
    }

    public class TalentProfileResponse
    {
        public List<TalentProfile> Items { get; set; }
    }

}
