namespace Beauty_Works.Models.Domain
{
    public class Subcategory
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int? ProductTypeID { get; set; }
        public ProductType? ProductType { get; set; }
        public bool? HasVariant { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
