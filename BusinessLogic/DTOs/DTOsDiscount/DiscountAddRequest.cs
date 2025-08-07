using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsDiscount
{
    public class DiscountAddRequest : AuditableRequest
    {
        public required string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public int Percentage { get; set; }
        public List<int> DiscountProductId { get; set; } = new List<int>();
        public List<int> DiscountClientId { get; set; } = new List<int>();
        public DiscountStatus Status { get; set; } = DiscountStatus.Available;
        public int? BrandId { get; set; } = null;
        public int? SubCategoryId { get; set; } = null;
        public int? ZoneId { get; set; } = null;
    }
}
