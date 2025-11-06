namespace ForumWebsite.DTO.Activity
{
    public class ReportDTO
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Reason { get; set; }

        public int UserID { get; set; }
        public int ThreadID { get; set; }
        public int StatusID { get; set; }
    }
}
