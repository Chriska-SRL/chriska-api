using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsProductItem;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class ReturnRequestUpdateRequest:AuditableRequest
    {
        public int Id { get; set; }
        public string Observations { get; set; }
        public List<ProductItemRequest> ProductItems { get; set; } = new List<ProductItemRequest>();
    }
}
