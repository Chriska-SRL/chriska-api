using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class ProductResponse : AuditableResponse
    {
        public int Id { get; set; }
        public required string InternalCode { get; set; }
        public required string Name { get; set; }
        public string? Barcode { get; set; }
        public UnitType UnitType { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public int EstimatedWeight { get; set; }
        public int Stock { get; set; }
        public int AvailableStock { get; set; }
        public required string ImageUrl { get; set; }
        public required string Observations { get; set; }
        public required SubCategoryResponse SubCategory { get; set; }
        public required BrandResponse Brand { get; set; }
        public required List<SupplierResponse> Suppliers { get; set; }
    }
}
