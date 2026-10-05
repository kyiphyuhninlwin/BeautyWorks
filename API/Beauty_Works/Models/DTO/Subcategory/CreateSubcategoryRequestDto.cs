namespace Beauty_Works.Models.DTO.Subcategory
{
    public class CreateSubcategoryRequestDto
    {
        public string? Name { get; set; }
        public bool? HasVariant { get; set; }
        public int? ProductTypeID { get; set; }
    }
}
