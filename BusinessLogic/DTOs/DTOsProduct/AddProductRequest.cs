using BusinessLogic.Común.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class ProductAddRequest : AuditableRequest
    {
        public string? Barcode { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int SubCategoryId { get; set; }
        public int BrandId { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public UnitType UnitType { get; set; }
        public int EstimatedWeight { get; set; } //Peso estimado en gramos
        public required string Observations { get; set; }
        public List<int> SupplierIds { get; set; } = new List<int>();
    }
}
