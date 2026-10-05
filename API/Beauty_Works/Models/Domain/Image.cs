namespace Beauty_Works.Models.Domain
{
    public class Image
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? FileExtension { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
        public Product? Product { get; set; }
    }
}
