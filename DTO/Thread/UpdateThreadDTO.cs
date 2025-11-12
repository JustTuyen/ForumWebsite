namespace ForumWebsite.DTO.Thread
{
    public class UpdateThreadDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public int? ImageID { get; set; }
        public int CategoryID { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public DateTime ExpirationAt { get; set; }
    }
}
