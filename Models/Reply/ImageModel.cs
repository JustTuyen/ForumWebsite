using ForumWebsite.Models.Thread;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumWebsite.Models.Reply
{
    public class ImageModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public string PublicID {  get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // forgein key
        public int? ThreadID { get; set; }
        [JsonIgnore]
        public virtual ThreadModel? Thread { get; set; }

        public int? ReplyID { get; set; }
        [JsonIgnore]
        public virtual ReplyModel? Reply { get; set; }
    }
}
