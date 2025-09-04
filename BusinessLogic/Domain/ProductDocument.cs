using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public abstract class ProductDocument : IEntity<ProductDocument.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Observations { get; set; }
        public User? User { get; set; }
        public List<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
        public AuditInfo? AuditInfo { get; set; }

        // Constructor para el alta
        public ProductDocument(string? observation, User user, List<ProductItem>? productItems)
        {
            Date = DateTime.Now;
            Observations = observation ?? "";
            User = user;
            ProductItems = productItems ?? new List<ProductItem>();
            AuditInfo = new AuditInfo();
        }
        // Constructor para la obtención
        public ProductDocument(int id, DateTime date, string observations, User? user, List<ProductItem> productItems, AuditInfo? auditInfo)
        {
            Id = id;
            Date = date;
            Observations = observations;
            User = user;
            ProductItems = productItems;
            AuditInfo = auditInfo;
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo?.SetDeleted(userId, location);
        }

        public abstract void Validate();

        public void Update(UpdatableData data)
        {
            Observations = data.Observations ?? Observations;
            User = data.User;
            ProductItems = data.ProductItems ?? ProductItems;
            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public decimal getAmount()
        {
            if (ProductItems == null || !ProductItems.Any())
                return 0m;

            return ProductItems.Sum(pi =>
            {
                decimal weight = pi.Weight ?? 0m;

                decimal baseValue = pi.Product.UnitType == UnitType.Unit
                    ? pi.UnitPrice * pi.Quantity
                    : pi.UnitPrice * (weight / 1000m);

                return baseValue * (1 - (pi.Discount / 100m));
            });
        }

        public class UpdatableData : AuditData
        {
            public string? Observations { get; set; }
            public User? User { get; set; }
            public List<ProductItem>? ProductItems { get; set; }


        }
    }
}