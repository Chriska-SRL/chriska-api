using BusinessLogic.DTOs.DTOsProduct;


namespace BusinessLogic.DTOs.DTOsProductItem
{
    public class ProductItemResponse
    {
        public int Quantity { get; set; }
        public int? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public ProductResponse? Product { get; set; }
    }
}
