using ForumWebsite.Models.Thread;
using ForumWebsite.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumWebsite.Models.Reply
{
    public class ReplyModel
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(1000)]
        public string Content { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = "Anonymous Melon";
        [Required]
        public int LikeCount { get; set; } = 0;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //forgein key
        //one

        public int? ImageID { get; set; }
        [JsonIgnore]
        public virtual ImageModel? Image { get; set; }


        [Required]
        public int UserID { get; set; }
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        public int? ParentReplyID { get; set; }
        [JsonIgnore]
        public virtual ReplyModel? ParentReply { get; set; }

        [Required]
        public int ThreadID { get; set; }
        [JsonIgnore]
        public virtual ThreadModel? Thread { get; set; }

        [Required]
        public int StatusID { get; set; }
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        //many
        public ICollection<UserActivityModel> UserActivites { get; set; } = new List<UserActivityModel>();
        public virtual ICollection<ReplyModel>? ChildReplies { get; set; } = new List<ReplyModel>();


    }
}
