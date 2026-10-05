namespace Beauty_Works.Models.DTO.Subcategory
{
    public class SubcategoryDto
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public bool? HasVariant { get; set; }
        public int? ProductTypeID { get; set; }
        public string? ProductTypeName { get; set; }
    }
}
