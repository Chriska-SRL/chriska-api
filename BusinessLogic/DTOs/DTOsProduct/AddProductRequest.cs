using BusinessLogic.Común.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class AddProductRequest : AuditableRequest
    {
        public required string Name { get; set; }
        public string? Barcode { get; set; }
        public UnitType UnitType { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public int Stock { get; set; }
        public int AvailableStock { get; set; }
        public required string Image { get; set; }
        public required string Observations { get; set; }
        public int SubCategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
