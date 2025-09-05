using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class Purchase : SupplierDocument
    {
        public string? InvoiceNumber { get; set; }
        public Status Status { get; set; }

        public Purchase(string? observation, User user, List<ProductItem> productItems, Supplier supplier, string invoiceNumber, Status status)
            : base(observation, user, productItems, supplier)
        {
            InvoiceNumber = invoiceNumber;
            Status = status;
            Validate();
        }

        public Purchase(int id, DateTime date, string observations, User? user, List<ProductItem> productItems, Supplier? supplier, AuditInfo? auditInfo, string? invoiceNumber, Status status)
            : base(id, date, observations, user, productItems, supplier, auditInfo)
        {
            InvoiceNumber = invoiceNumber;
            Status = status;
        }

        public override void Validate()
        {
            if (!string.IsNullOrWhiteSpace(Observations) && Observations.Length > 255)
                throw new ArgumentException("La observación no puede superar los 255 caracteres.");
            if (Supplier == null) throw new ArgumentException("El proveedor es obligatorio.");
            if (!string.IsNullOrWhiteSpace(InvoiceNumber) && InvoiceNumber.Length > 30)
                throw new ArgumentException("El número de factura no puede superar los 30 caracteres.");
        }

        public class UpdatableData : AuditData
        {
            public string? Observations { get; set; }
            public string? InvoiceNumber { get; set; }
            public List<ProductItem>? ProductItems { get; set; }
            public Supplier? Supplier { get; set; }
        }

        public void Update(UpdatableData data)
        {
            Observations = data.Observations ?? Observations;
            InvoiceNumber = data.InvoiceNumber ?? InvoiceNumber;
            ProductItems = data.ProductItems ?? ProductItems;
            Supplier = data.Supplier ?? Supplier;
            AuditInfo?.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        internal void Confirm()
        {
            Status = Status.Confirmed;
        }

        internal void Cancel()
        {
            Status = Status.Cancelled;
        }
    }
}
