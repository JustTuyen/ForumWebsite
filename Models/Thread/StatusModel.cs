using ForumWebsite.Models.Activity;
using ForumWebsite.Models.Reply;
using ForumWebsite.Models.User;
using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.Thread
{
    public class StatusModel
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(255)]
        public string StatusName {  get; set; }

        [Required, MaxLength(255)]
        public string About {  get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //forgein key
        public ICollection<ThreadModel> Threads { get; set; } = new List<ThreadModel>();
        public ICollection<ReportModel> Reports { get; set; } = new List<ReportModel>();
        public ICollection<ReplyModel> Replies { get; set; } = new List<ReplyModel>();
        public ICollection<UserModel> Users { get; set; } = new List<UserModel>();

    }
}
