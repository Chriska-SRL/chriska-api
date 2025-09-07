
namespace BusinessLogic.DTOs.DTOsProductItem
{
    public class ProductItemRequest
    { 
        public decimal Quantity { get; set; }
        public int ProductId { get; set; }
    }

    public class ProductItemRequestForOrder
    {
        public decimal Quantity { get; set; }
        public int? Weight { get; set; }
        public int ProductId { get; set; }
    }

    public class ProductItemRequestForPurchase
    {
        public decimal Quantity { get; set; }
        public int? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public decimal Discount { get; set; }
    }
}
