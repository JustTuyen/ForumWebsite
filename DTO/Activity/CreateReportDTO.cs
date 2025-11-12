namespace ForumWebsite.DTO.Activity
{
    public class CreateReportDTO
    {
        public string Reason { get; set; }
        public int UserID { get; set; }
        public int ThreadID { get; set; }
        public int StatusID { get; set; }
    }
}
