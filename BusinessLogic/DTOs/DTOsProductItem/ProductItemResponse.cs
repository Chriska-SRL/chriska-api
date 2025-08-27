using BusinessLogic.DTOs.DTOsProduct;


namespace BusinessLogic.DTOs.DTOsProductItem
{
    public class ProductItemResponse
    {
        public decimal Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public ProductResponse? Product { get; set; }
    }
}
