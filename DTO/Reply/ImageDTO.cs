namespace ForumWebsite.DTO.Reply
{
    public class ImageDTO
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string PublicID { get; set; }

        //public DateTime CreatedAt { get; set; }
        public int? ThreadID { get; set; }
        public int? ReplyID { get; set; }
    }
}
