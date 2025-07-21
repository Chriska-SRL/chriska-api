
using BusinessLogic.Común;

namespace BusinessLogic.DTOs.DTOsDiscount
{
    public class DiscountResponse
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public int Percentage { get; set; }
        public AuditInfo AuditInfo { get; set; }
    }
}
