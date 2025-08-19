using BusinessLogic.DTOs.DTOsAudit;

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
}
