using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.DTOs.DTOsDiscount
{
    public class DiscountResponse: AuditableResponse
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Percentage { get; set; }
        public List<ProductResponse>? Products{ get; set; } 
        public List<ClientResponse>? Clients { get; set; }
        public DiscountStatus Status { get; set; } 
        public BrandResponse? Brand { get; set; } = null;
        public SubCategoryResponse? SubCategory { get; set; } = null;
        public ZoneResponse? Zone { get; set; } = null;
    }
}
