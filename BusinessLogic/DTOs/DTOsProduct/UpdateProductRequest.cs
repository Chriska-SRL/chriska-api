using BusinessLogic.Común.Enums;

namespace BusinessLogic.DTOs.DTOsProduct
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public UnitType UnitType { get; set; }
        public string Description { get; set; }
        public TemperatureCondition TemperatureCondition { get; set; }
        public string Observation { get; set; }
        public int SubCategoryId { get; set; }
    }
}
