using BusinessLogic.Común.Enums;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Barcode { get; set; }
        public UnitType UnitType { get; set; }
        public decimal Price { get; set; }
        public int SubCategoryId { get; set; }
        public int BrandId { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public string? Observation { get; set; }
        public string? Image { get; set; }
    }
}
