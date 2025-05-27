namespace IHCApis.Models
{
    public class ConsultationFormModel
    {
        // Subject
        public string Subject { get; set; }

        // Message from business
        public string Message { get; set; }

        // Comments
        public string Comments { get; set; }

        // Attachments
        // Use IFormFileCollection to bind multiple uploaded files
        public string FilesPath { get; set; }
    }
}
