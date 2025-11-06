namespace ForumWebsite.DTO.Activity
{
    public class BookmarkDTO
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Remark { get; set; }
        public int UserID { get; set; }
        public int ThreadID { get; set; }
    }
}
