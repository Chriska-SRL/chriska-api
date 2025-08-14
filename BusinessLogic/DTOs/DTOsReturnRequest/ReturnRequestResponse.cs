using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsProductItem;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class ReturnRequestResponse:AuditableResponse
    {
        public int Id { get; set; }
        public ClientResponse? Client { get; set; }
        public string? Observation { get; set; } 
        public Status Status { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public UserResponse? User { get; set; }
        public List<ProductItemResponse> ProductItems { get; set; } = new();
        public DeliveryResponse? Delivery { get; set; }
    }
}
