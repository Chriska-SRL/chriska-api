using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class ReturnPurchase : SupplierDocument
    {
        public ReturnPurchase(string? observation, User user, List<ProductItem>? productItems, Supplier supplier) : base(observation, user, productItems, supplier)
        {
        }

        public ReturnPurchase(int id, DateTime date, string observations, User? user, List<ProductItem> productItems, Supplier? supplier, AuditInfo? auditInfo) : base(id, date, observations, user, productItems, supplier, auditInfo)
        {
        }
    }
}
