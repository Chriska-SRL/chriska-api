using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsProductItem;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.DTOs.DTOsOrderRequest
{
    public class OrderRequestResponse: AuditableResponse
    {
        public int Id { get; set; }
        public string Observations { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public Status Status { get; set; }
        public UserResponse? User { get; set; }
        public ClientResponse? Client { get; set; }
        //public OrderResponse Order { get; set; } = new OrderResponse();
        public List<ProductItemResponse> ProductItems { get; set; } = new List<ProductItemResponse>();
    }
}
