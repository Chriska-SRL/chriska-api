using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDiscount
{
    public class DiscountUpdateRequest : AuditableRequest
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public int Percentage { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
        public List<int> ClientIds { get; set; } = new List<int>();
        public DiscountStatus Status { get; set; } = DiscountStatus.Available;
        public int? BrandId { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
