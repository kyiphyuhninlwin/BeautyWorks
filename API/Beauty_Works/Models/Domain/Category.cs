namespace Beauty_Works.Models.Domain
{
    public class Category
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public ICollection<ProductType>? ProductTypes { get; set; }
    }
}
