using ForumWebsite.Models.Thread;
using ForumWebsite.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumWebsite.Models.Activity
{
    public class ReportModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(255)]
        public string Reason { get; set; }

        //fogein key
        [Required]
        public int UserID { get; set; }
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        [Required]
        public int ThreadID { get; set; }
        [JsonIgnore]
        public virtual ThreadModel? Thread { get; set; }

        [Required]
        public int StatusID { get; set; }
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }
    }
}
