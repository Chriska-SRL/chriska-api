using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsAudit;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class ReturnRequestAddRequest:AuditableRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Observation { get; set; } = string.Empty;
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public int UserId { get; set; }
        public List<int> ProductItemsId { get; set; } = new();
        public int DeliveryId { get; set; }
    }

}
