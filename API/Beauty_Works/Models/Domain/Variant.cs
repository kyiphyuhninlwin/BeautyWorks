namespace Beauty_Works.Models.Domain
{
    public class Variant
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int? SubcategoryID { get; set; }
        public Subcategory? Subcategory { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
