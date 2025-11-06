namespace ForumWebsite.DTO.Thread
{
    public class StatusDTO
    {
        public int ID { get; set; }
        public string StatusName {  get; set; }
        public string About {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
