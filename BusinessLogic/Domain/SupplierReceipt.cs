using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class SupplierReceipt : Receipt
    {
        public Supplier? Supplier { get; set; }
        public SupplierReceipt(DateTime date, decimal amount, string notes, PaymentMethod paymentMethod, Supplier client) : base(date, amount, notes, paymentMethod)
        {
            Supplier = client ?? throw new ArgumentException(nameof(client), "El proveedor es obligatorio.");
        }

        public SupplierReceipt(int id, DateTime date, decimal amount, string notes, PaymentMethod paymentMethod, AuditInfo? auditInfo, Supplier? client) : base(id, date, amount, notes, paymentMethod, auditInfo)
        {
            Supplier = client;
        }
    }
}
