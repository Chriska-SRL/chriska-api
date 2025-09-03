
namespace BusinessLogic.DTOs.DTOsProductItem
{
    public class ProductItemRequest
    { 
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }

    public class ProductItemRequestForOrder
    {
        public int Quantity { get; set; }
        public int? Weight { get; set; }
        public int ProductId { get; set; }
    }

    public class ProductItemRequestForPurchase
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public decimal Discount { get; set; }
    }
}
