namespace ForumWebsite.DTO.Thread
{
    public class LikeAndStatusThreadDTO
    {
        public int ID { get; set; }
        //public int? LikeCount {  get; set; }
        public int? StatusID {  get; set; }

        public DateTime? ExpirationAt { get; set; }
    }
}
