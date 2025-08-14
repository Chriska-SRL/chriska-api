using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public abstract class ProductDocument:IAuditable
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Observation { get; set; }
        public User? User { get; set; }
        public List<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
        public AuditInfo? AuditInfo { get; set; }

        public ProductDocument(DateTime? date, string? observation, User? user, List<ProductItem> productItems, AuditInfo? auditInfo)
        {
            Date = date;
            Observation = observation;
            User = user;
            ProductItems = productItems ?? new List<ProductItem>();
            AuditInfo =  new AuditInfo();
        }
        public ProductDocument(int id,DateTime? date, string? observation, User? user, List<ProductItem> productItems, AuditInfo? auditInfo)
        {
            Date = date;
            Observation = observation;
            User = user;
            ProductItems = productItems;
            AuditInfo = auditInfo;
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo?.SetDeleted(userId, location);
        }
    }
}
