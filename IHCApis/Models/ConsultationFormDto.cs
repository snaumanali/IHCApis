namespace iHC.Models.Forms
{
    public class ConsultationFormDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Comments { get; set; }
        public string FilesPath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
