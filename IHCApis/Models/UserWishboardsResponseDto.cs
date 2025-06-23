namespace iHC.Models.Wishboard
{
    public class UserWishboardsResponseDto
    {
        public int Id { get; set; }
        public int WishboardId { get; set; }
        public string OptionId { get; set; }
        public string OptionTitle { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProfileImage { get; set; }
        public string WishEmail { get; set; }
    }
}
