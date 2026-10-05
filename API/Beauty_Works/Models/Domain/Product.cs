namespace Beauty_Works.Models.Domain
{
    public class Product
    {
        public int ID { get; set; }
        public int? OrderID { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Desp { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? SubcategoryID { get; set; }
        public Subcategory? Subcategory { get; set; }
        public int? StatusID { get; set; }
        public Status? Status { get; set; }
        public int? BrandID { get; set; }
        public Brand? Brand { get; set; }
        public int? ImageID { get; set; }
        public Image? Image { get; set; }
        public ICollection<Variant>? Variants { get; set; }
    }
}
