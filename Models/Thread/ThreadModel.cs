using ForumWebsite.Models.Activity;
using ForumWebsite.Models.Reply;
using ForumWebsite.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ForumWebsite.Models.Thread
{
    public class ThreadModel
    {
        [Key]
        public int ID {  get; set; }

        [Required,MaxLength(255)]
        public string Name { get; set; } = "Anonymous Melon";

        [Required, MaxLength(1000)]
        public string Title { get; set; }
        [Required, MaxLength(1000)]
        public string Content {  get; set; }

        [Required]
        public int LikeCount { get; set; } = 0;
        [Required]
        public int ViewCount { get; set; } = 0;
        [Required]
        public int ReplyLimit { get; set; } = 200;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;

        //Forgein keys
        //one
        [Required]
        public int UserID { get; set; }
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        [Required]
        public int CategoryID { get; set; }
        [JsonIgnore]
        public virtual CategoryModel? Category { get; set; }

        [Required]
        public int StatusID { get; set; }
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public int? ImageID { get; set; }
        [JsonIgnore]
        public virtual ImageModel? Image { get; set; }

        //many
        public ICollection<ReplyModel> Replies { get; set; } = new List<ReplyModel>();
        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();
        public ICollection<BookmarkModel> Bookmarks { get; set; } = new List<BookmarkModel>();
        public ICollection<UserActivityModel> UserActivites { get; set; } = new List<UserActivityModel>();
        public ICollection<ReportModel> Reports { get; set; } = new List<ReportModel>();
    }
}
