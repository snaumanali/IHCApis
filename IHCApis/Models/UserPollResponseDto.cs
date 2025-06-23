namespace iHC.Models.Forms
{
    public class UserPollResponseDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string OptionTitle { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OptionId { get; set; }
        public string ProfileImage { get; set; }
    }
}
