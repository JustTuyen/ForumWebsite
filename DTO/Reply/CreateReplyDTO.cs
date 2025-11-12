namespace ForumWebsite.DTO.Reply
{
    public class CreateReplyDTO
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public int LikeCount { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //
        public int UserID { get; set; }
        public int? ParentReplyID { get; set; }
        public int ThreadID { get; set; }
        public int StatusID { get; set; }

    }
}
