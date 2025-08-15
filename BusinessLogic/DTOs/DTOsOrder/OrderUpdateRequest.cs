using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.DTOs.DTOsOrder
{
    public class OrderUpdateRequest:AuditableRequest
    {
        public int Id { get; set; }
        public string Observations { get; set; }
        public int Crates { get; set; }
        public List<ProductItemRequest> ProductItems { get; set; } = new List<ProductItemRequest>();
    }
}
