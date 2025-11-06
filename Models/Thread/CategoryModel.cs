using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.Thread
{
    public class CategoryModel
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(100)]
        public string Name {  get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        //Forgein key
        public ICollection<ThreadModel> Threads { get; } = new List<ThreadModel>();
    }
}
