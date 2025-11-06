using ForumWebsite.Models.Thread;
using ForumWebsite.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumWebsite.Models.Activity
{
    public class BookmarkModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Remark {  get; set; }

        //forgein key
        [Required]
        public int UserID { get; set; }
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        [Required]
        public int ThreadID { get; set; }
        [JsonIgnore]
        public virtual ThreadModel? Thread { get; set; }
    }
}

