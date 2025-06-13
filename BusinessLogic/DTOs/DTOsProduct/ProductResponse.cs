using BusinessLogic.Común.Enums;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InternalCode { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public UnitType UnitType { get; set; }
        public string Description { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public string Observation { get; set; }
        public SubCategoryResponse SubCategory { get; set; }
        public List<SupplierResponse> Suppliers { get; set; } = new List<SupplierResponse>();
    }
}
