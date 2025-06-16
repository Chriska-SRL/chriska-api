using BusinessLogic.Común.Enums;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string InternalCode { get; set; }
        public required string Barcode { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public UnitType UnitType { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public required string Observation { get; set; }
        public required string Image { get; set; }
        public required SubCategoryResponse SubCategory { get; set; }
        public List<SupplierResponse> Suppliers { get; set; } = new List<SupplierResponse>();
    }
}
