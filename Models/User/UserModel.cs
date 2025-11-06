using ForumWebsite.Models.Activity;
using ForumWebsite.Models.Reply;
using ForumWebsite.Models.Thread;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ForumWebsite.Models.User
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; } 

        [Required, MaxLength(100)]
        public string Password { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //Forgein keys
        //one
        [Required]
        public int StatusID { get; set; }
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }
        //many
        public ICollection<ThreadModel> Threads { get; set; } = new List<ThreadModel>();
        public ICollection<ReplyModel> Replies { get; } = new List<ReplyModel>();
        public ICollection<BookmarkModel> Bookmarks { get; } = new List<BookmarkModel>();  
        public ICollection<LikeModel> Likes { get; } = new List<LikeModel>();
        public ICollection<ReportModel> Reports { get; } = new List<ReportModel>();
        public ICollection<UserActivityModel> UserActivities { get; } = new List<UserActivityModel>();

    }
}
