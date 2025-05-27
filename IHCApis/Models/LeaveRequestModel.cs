namespace IHCApis.Models
{
    public class LeaveRequestModel
    {
        public int PersonNumber { get; set; } = 1234;
        public string Employer { get; set; }
        public string AbsenceType { get; set; }
        public string StartDate { get; set; } // or DateTime if you prefer
        public string EndDate { get; set; } // or DateTime if you prefer
        public string StartTime { get; set; } = "00:00";
        public int Duration { get; set; }
        public string UnitOfMeasure { get; set; } = "D";
        public string AbsenceStatusCd { get; set; } = "SUBMITTED";
        public string SingleDayFlag { get; set; } = "N";
        public int StartDateDuration { get; set; }
    }

}
