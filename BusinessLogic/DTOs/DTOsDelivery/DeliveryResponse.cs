using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsProductItem;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.DTOs.DTOsDelivery
{
    public class DeliveryResponse:AuditableResponse
    {
        public int Id { get; set; }
        public ClientResponse Client { get; set; } = default!;
        public Status Status { get; set; } = default!;
        public DateTime? Date { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public string Observations { get; set; } = default!;
        public UserResponse User { get; set; } = default!;
        public List<ProductItemResponse> ProductItems { get; set; } = new();
        public int Crates { get; set; }
        public decimal Payment {  get; set; }
        public OrderResponse? Order { get; set; }
    }
}
