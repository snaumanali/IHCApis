namespace iHC.Models.Forms
{
    public class LineManager
    {
        public string ManagerName { get; set; }
    }

    public class PublicWorker
    {
        public long PersonId { get; set; }
        public string PersonNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public List<LineManager> Assignments { get; set; }
    }

    public class PublicWorkerResponse
    {
        public List<PublicWorker> Items { get; set; }
    }

}
