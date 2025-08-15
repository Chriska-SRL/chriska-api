using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.DTOs.DTOsOrderRequest
{
    public class OrderRequestAddRequest:AuditableRequest
    {
        public string Observations { get; set; } 
        public int ClientId { get; set; }
        public List<ProductItemRequest> ProductItems { get; set; } = new List<ProductItemRequest>();

    }
}
