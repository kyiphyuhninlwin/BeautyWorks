using Beauty_Works.Models.DTO.Variant;

namespace Beauty_Works.Models.DTO.Product
{
    public class ProductDto
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
        public string? SubcategoryName { get; set; }
        public int? StatusID { get; set; }
        public string? StatusName { get; set; }
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public int? ImageID { get; set; }
        public string? ImageName { get; set; }
        public List<VariantDto> Variants { get; set; } = new List<VariantDto>();
    }
}
