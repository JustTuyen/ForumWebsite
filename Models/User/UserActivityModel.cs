using ForumWebsite.Models.Reply;
using ForumWebsite.Models.Thread;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumWebsite.Models.User
{
    public class UserActivityModel
    {
        [Key]
        public int ID {  get; set; }
        [Required, MaxLength(200)]
        public string ActionType { get; set; }

        [MaxLength(255)]
        public string? IPAddress { get; set; }
        [MaxLength(255)]
        public string? UserAgent { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //fogein keys
        public int? UserID { get; set; }
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        public int? ThreadID { get; set; }
        [JsonIgnore]
        public virtual ThreadModel? Thread { get; set; }

        public int? ReplyID { get; set; }
        [JsonIgnore]
        public virtual ReplyModel? Reply { get; set; }
    }
}
