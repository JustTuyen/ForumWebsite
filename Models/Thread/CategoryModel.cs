using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.Thread
{
    public class CategoryModel
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(255)]
        public string Name {  get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        //Forgein key
        public ICollection<ThreadModel> Threads { get; } = new List<ThreadModel>();
    }
}
