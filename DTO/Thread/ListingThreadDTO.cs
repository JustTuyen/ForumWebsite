namespace ForumWebsite.DTO.Thread
{
    public class ListingThreadDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpirationAt { get; set; }
        //
        public int StatusID { get; set; }
        public int? ImageID { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryID { get; set; }
    }
}
