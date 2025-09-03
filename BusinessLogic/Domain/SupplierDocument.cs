
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class SupplierDocument: ProductDocument
    {
        public Supplier? Supplier { get; set; }
        public SupplierDocument(string? observation, User user, List<ProductItem>? productItems, Supplier supplier) : base(observation, user, productItems)
        {
            Supplier = supplier;
        }

        public SupplierDocument(int id, DateTime date, string observations, User? user, List<ProductItem> productItems, Supplier? supplier, AuditInfo? auditInfo) : base(id, date, observations, user, productItems, auditInfo)
        {
            Supplier = supplier;
        }

        public override void Validate()
        {
            
        }
    }
}
