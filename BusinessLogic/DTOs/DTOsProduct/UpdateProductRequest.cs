using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class ProductUpdateRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Barcode { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public TemperatureCondition? TemperatureCondition { get; set; }
        public int? EstimatedWeight { get; set; } //Peso estimado en gramos
        public string? Observations { get; set; }
        public int? SubCategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? ShelveId { get; set; }
        public List<int>? SupplierIds { get; set; }
    }
}
