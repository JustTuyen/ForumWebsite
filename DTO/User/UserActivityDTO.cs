using ForumWebsite.Models.Thread;

namespace ForumWebsite.DTO.User
{
    public class UserActivityDTO
    {
        public int ID { get; set; }
        public string ActionType { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        //public DateTime CreatedAt { get; set; }
        public int? UserID { get; set; }
        public int? ThreadID { get; set; }
        public int? ReplyID { get; set; }
    }
}
