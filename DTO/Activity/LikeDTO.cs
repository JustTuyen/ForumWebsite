namespace ForumWebsite.DTO.Activity
{
    public class LikeDTO
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserID { get; set; }
        public int ThreadID { get; set; }
    }
}
